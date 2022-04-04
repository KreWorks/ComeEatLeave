using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
	[SerializeField] private float FOVAngle;
	[SerializeField] private float smoothDamp;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private List<Vector3> directionsToCheckWhenAvoidingObstacles;

	private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
	private List<FlockUnit> avoidanceNeighbours = new List<FlockUnit>();
	private List<FlockUnit> aligementNeighbours = new List<FlockUnit>();
	private Flock assignedFlock;
	private Vector3 currentVelocity;
	private Vector3 currentObstacleAvoidanceVector;
	private float speed;

	public Transform myTransform { get; set; }
	public List<CornController> corns;
	private CornController targetCorn;

	private void Awake()
	{
		myTransform = transform;
		corns = new List<CornController>();
		GetCorns();
	}

	public void AssignFlock(Flock flock)
	{
		assignedFlock = flock;
	}

	public void InitializeSpeed(float speed)
	{
		this.speed = speed;
	}

	public void MoveUnit()
	{
		FindNeighbours();
		CalculateSpeed();

		var cohesionVector = CalculateCohesionVector() * assignedFlock.cohesionWeight;
		var avoidanceVector = CalculateAvoidanceVector() * assignedFlock.avoidanceWeight;
		var aligementVector = CalculateAligementVector() * assignedFlock.aligementWeight;
		var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;
		var obstacleVector = CalculateObstacleVector() * assignedFlock.obstacleWeight;
		var cornVector = CalculateCornAlive() * 0.5f;

		var moveVector = cornVector + cohesionVector + avoidanceVector + aligementVector + boundsVector + obstacleVector;
		moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
		moveVector = moveVector.normalized * speed;
		if (moveVector == Vector3.zero)
			moveVector = transform.forward;

		myTransform.forward = moveVector;
		if (myTransform.position.y < 0)
		{
			moveVector.y = Mathf.Abs(moveVector.y);
		}

		//Debug.DrawLine(myTransform.position, myTransform.position + moveVector, Color.white, 2f);
		myTransform.position += moveVector * Time.deltaTime;
	}

	public void SetDirectionsToCheckWhenAvoidingObstacles(List<Vector3> directions)
	{
		directionsToCheckWhenAvoidingObstacles = directions;
	}

	private void GetCorns()
	{
		corns.AddRange(FindObjectsOfType<CornController>());
		if (corns.Count == 0)
		{
			targetCorn = null;
		}
		else
		{
			targetCorn = corns[UnityEngine.Random.Range(0, corns.Count)];
		}
	}

	private void FindNeighbours()
	{
		cohesionNeighbours.Clear();
		avoidanceNeighbours.Clear();
		aligementNeighbours.Clear();
		var allUnits = assignedFlock.allUnits;
		for (int i = 0; i < allUnits.Count; i++)
		{
			var currentUnit = allUnits[i];
			if (currentUnit != this)
			{
				float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.myTransform.position - myTransform.position);
				if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
				{
					cohesionNeighbours.Add(currentUnit);
				}
				if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
				{
					avoidanceNeighbours.Add(currentUnit);
				}
				if (currentNeighbourDistanceSqr <= assignedFlock.aligementDistance * assignedFlock.aligementDistance)
				{
					aligementNeighbours.Add(currentUnit);
				}
			}
		}
	}

	private void CalculateSpeed()
	{
		if (cohesionNeighbours.Count == 0)
			return;
		speed = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			speed += cohesionNeighbours[i].speed;
		}

		speed /= cohesionNeighbours.Count;
		speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
	}

	private Vector3 CalculateCohesionVector()
	{
		var cohesionVector = Vector3.zero;
		if (cohesionNeighbours.Count == 0)
			return Vector3.zero;
		int neighboursInFOV = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			if (IsInFOV(cohesionNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				cohesionVector += cohesionNeighbours[i].myTransform.position;
			}
		}

		cohesionVector /= neighboursInFOV;
		cohesionVector -= myTransform.position;
		cohesionVector = cohesionVector.normalized;
		return cohesionVector;
	}

	private Vector3 CalculateAligementVector()
	{
		var aligementVector = myTransform.forward;
		if (aligementNeighbours.Count == 0)
			return myTransform.forward;
		int neighboursInFOV = 0;
		for (int i = 0; i < aligementNeighbours.Count; i++)
		{
			if (IsInFOV(aligementNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				aligementVector += aligementNeighbours[i].myTransform.forward;
			}
		}

		aligementVector /= neighboursInFOV;
		aligementVector = aligementVector.normalized;
		return aligementVector;
	}

	private Vector3 CalculateAvoidanceVector()
	{
		var avoidanceVector = Vector3.zero;
		if (aligementNeighbours.Count == 0)
			return Vector3.zero;
		int neighboursInFOV = 0;
		for (int i = 0; i < avoidanceNeighbours.Count; i++)
		{
			if (IsInFOV(avoidanceNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
			}
		}

		avoidanceVector /= neighboursInFOV;
		avoidanceVector = avoidanceVector.normalized;
		return avoidanceVector;
	}

	private Vector3 CalculateBoundsVector()
	{
		var offsetToCenter = assignedFlock.transform.position - myTransform.position;
		bool isNearCenter = (offsetToCenter.magnitude >= assignedFlock.boundsDistance * 0.9f);
		return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
	}

	private Vector3 CalculateObstacleVector()
	{
		var obstacleVector = Vector3.zero;
		RaycastHit hit;
		if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
		{
			obstacleVector = FindBestDirectionToAvoidObstacle();
		}
		else
		{
			currentObstacleAvoidanceVector = Vector3.zero;
		}
		return obstacleVector;
	}

	private Vector3 FindBestDirectionToAvoidObstacle()
	{
		if (currentObstacleAvoidanceVector != Vector3.zero)
		{
			RaycastHit hit;
			if (!Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
			{
				return currentObstacleAvoidanceVector;
			}
		}
		float maxDistance = int.MinValue;
		var selectedDirection = Vector3.zero;
		for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Count; i++)
		{

			RaycastHit hit;
			var currentDirection = myTransform.TransformDirection(directionsToCheckWhenAvoidingObstacles[i].normalized);
			if (Physics.Raycast(myTransform.position, currentDirection, out hit, assignedFlock.obstacleDistance, obstacleMask))
			{

				float currentDistance = (hit.point - myTransform.position).sqrMagnitude;
				if (currentDistance > maxDistance)
				{
					maxDistance = currentDistance;
					selectedDirection = currentDirection;
				}
			}
			else
			{
				selectedDirection = currentDirection;
				currentObstacleAvoidanceVector = currentDirection.normalized;
				return selectedDirection.normalized;
			}
		}
		return selectedDirection.normalized;
	}

	private Vector3 CalculateCornAlive()
	{
		if (targetCorn.IsDead)
		{
			if (corns.Count == 0)
			{
				GetCorns();
			}
			RemoveDeletedCorns();

			targetCorn = corns[UnityEngine.Random.Range(0, corns.Count)];
			if (corns.Count < 100)
			{
				float closestDistanceSqrt = Vector3.SqrMagnitude(targetCorn.transform.position - myTransform.position);
				foreach (CornController corn in corns)
				{
					float currentDistanceSqrt = Vector3.SqrMagnitude(corn.transform.position - myTransform.position);
					if (currentDistanceSqrt < closestDistanceSqrt)
					{
						targetCorn = corn;
						closestDistanceSqrt = currentDistanceSqrt;
					}
				}
			}
		}

		Vector3 cornDirection = targetCorn.transform.position - myTransform.position;

		return Vector3.Normalize(cornDirection);
	}

	private void RemoveDeletedCorns()
	{
		List<CornController> newCorns = new List<CornController>();
		foreach(CornController corn in corns)
		{
			if (!corn.IsDead)
			{
				newCorns.Add(corn);
			}
		}

		corns = newCorns;
	}

	private bool IsInFOV(Vector3 position)
	{
		return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
	}
}

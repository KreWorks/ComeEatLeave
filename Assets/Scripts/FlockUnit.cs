using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
	[SerializeField] private float FOVAngle;
	[SerializeField] private float smoothDamp;

	private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
	private List<FlockUnit> avoidanceNeighbours = new List<FlockUnit>();
	private List<FlockUnit> alignmentNeighbours = new List<FlockUnit>();
	private Flock assignedFlock;
	private Vector3 currentVelocity;
	private float speed;
	private Vector3 directionPosition;

	public Transform myTransform;

	public void Awake()
	{
		this.myTransform = this.transform;
	}

	public void AssignFlock(Flock flock)
	{
		assignedFlock = flock;
	}

	public void InitializeSpeed(float speed, Vector3 direction)
	{
		this.speed = speed;
		this.directionPosition = direction;
	}

	public void MoveUnit()
	{
		FindNeighbours();
		CalculateSpeed();

		Vector3 directionVector = Vector3.Normalize(directionPosition - myTransform.position) * 1f;

		Vector3 cohesionVector = CalculateCohesionVector() * assignedFlock.cohesionWeight;
		Vector3 avoidanceVector = CalculateAvoidanceVector() * assignedFlock.avoidanceWeight;
		Vector3 alignmentVector = CalculateAlignmentVector() * assignedFlock.alignmentWeight;
		

		Vector3 moveVector = cohesionVector + avoidanceVector + alignmentVector;
		myTransform.forward = moveVector;
		moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
		moveVector = moveVector.normalized * speed;

		if (myTransform.position.y < 0  || myTransform.position.y > 0.5)
		{
			moveVector.y = -moveVector.y;
		}
		myTransform.forward = moveVector;
		myTransform.position += moveVector * Time.deltaTime;
	}

	private void FindNeighbours()
	{
		cohesionNeighbours.Clear();
		avoidanceNeighbours.Clear();
		alignmentNeighbours.Clear();

		var allUnits = assignedFlock.allUnits;
		for (int i = 0; i < allUnits.Length; i++)
		{
			var currentUnit = allUnits[i]; 
			if (currentUnit != this)
			{
				float currentNeightbourDistanceSqrt = Vector3.SqrMagnitude(currentUnit.transform.position - myTransform.position);
				if (currentNeightbourDistanceSqrt <= Mathf.Pow(assignedFlock.cohesionDistance, 2))
				{
					cohesionNeighbours.Add(currentUnit);
				}
				if (currentNeightbourDistanceSqrt <= Mathf.Pow(assignedFlock.avoidanceDistance, 2))
				{
					avoidanceNeighbours.Add(currentUnit);
				}
				if (currentNeightbourDistanceSqrt <= Mathf.Pow(assignedFlock.alignmentDistance, 2))
				{
					alignmentNeighbours.Add(currentUnit);
				}
			}
		}
	}

	private void CalculateSpeed()
	{
		if (cohesionNeighbours.Count == 0)
		{
			return;
		}

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
		Vector3 cohesionVecrtor = Vector3.zero;

		if (cohesionNeighbours.Count == 0)
		{
			return cohesionVecrtor;
		}

		int neighboursInFOV = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			if(IsInFOV(cohesionNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				cohesionVecrtor += cohesionNeighbours[i].myTransform.position;
			}
		}

		if (neighboursInFOV == 0)
		{
			return cohesionVecrtor;
		}
		else
		{
			return Vector3.Normalize((cohesionVecrtor / neighboursInFOV) - myTransform.position);
		}
		
	}

	private Vector3 CalculateAlignmentVector()
	{
		Vector3 alignmentVector = myTransform.forward;
		if(alignmentNeighbours.Count == 0)
		{
			return alignmentVector;
		}

		int neighboursInFOV = 0;
		for (int i = 0; i < alignmentNeighbours.Count; i++)
		{
			if (IsInFOV(alignmentNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				alignmentVector += alignmentNeighbours[i].myTransform.forward;
			}
		}

		return Vector3.Normalize((alignmentVector / neighboursInFOV));
	}

	private Vector3 CalculateAvoidanceVector()
	{
		Vector3 avoidanceVector = Vector3.zero;
		if (avoidanceNeighbours.Count == 0)
		{
			return avoidanceVector;
		}

		int neighboursInFOV = 0;
		for (int i = 0; i < avoidanceNeighbours.Count; i++)
		{
			if (IsInFOV(avoidanceNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
			}
		}

		return Vector3.Normalize((avoidanceVector / neighboursInFOV));
	}

	private bool IsInFOV(Vector3 position)
	{
		return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
	}
}

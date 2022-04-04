using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	[Header("Spawn Setup")]
	[SerializeField] private FlockUnit flockUnitPrefab;
	[SerializeField] private int flockSize;
	[SerializeField] private Vector3 spawnBounds;
	[SerializeField] private List<Vector3> spawnPoints;

	[Header("Speed Setup")]
	[Range(0, 10)]
	[SerializeField] private float _minSpeed;
	public float minSpeed { get { return _minSpeed; } }
	[Range(0, 10)]
	[SerializeField] private float _maxSpeed;
	public float maxSpeed { get { return _maxSpeed; } }


	[Header("Detection Distances")]

	[Range(0, 10)]
	[SerializeField] private float _cohesionDistance = 6;
	public float cohesionDistance { get { return _cohesionDistance; } }

	[Range(0, 10)]
	[SerializeField] private float _avoidanceDistance = 6;
	public float avoidanceDistance { get { return _avoidanceDistance; } }

	[Range(0, 10)]
	[SerializeField] private float _aligementDistance = 6;
	public float aligementDistance { get { return _aligementDistance; } }

	[Range(0, 10)]
	[SerializeField] private float _obstacleDistance = 1;
	public float obstacleDistance { get { return _obstacleDistance; } }

	[Range(0, 100)]
	[SerializeField] private float _boundsDistance = 3;
	public float boundsDistance { get { return _boundsDistance; } }


	[Header("Behaviour Weights")]

	[Range(0, 10)]
	[SerializeField] private float _cohesionWeight = 1;
	public float cohesionWeight { get { return _cohesionWeight; } }

	[Range(0, 10)]
	[SerializeField] private float _avoidanceWeight = 1;
	public float avoidanceWeight { get { return _avoidanceWeight; } }

	[Range(0, 10)]
	[SerializeField] private float _aligementWeight = 1;
	public float aligementWeight { get { return _aligementWeight; } }

	[Range(0, 10)]
	[SerializeField] private float _boundsWeight = 1;
	public float boundsWeight { get { return _boundsWeight; } }

	[Range(0, 100)]
	[SerializeField] private float _obstacleWeight = 1;
	public float obstacleWeight { get { return _obstacleWeight; } }

	public List<FlockUnit> allUnits { get; set; }

	int locustCount = 0;
	private void Start()
	{
		locustCount = 0;
		//GenerateUnits();
	}

	private void Update()
	{
		if(allUnits != null)
		{
			for (int i = 0; i < allUnits.Count; i++)
			{
				allUnits[i].MoveUnit();
			}
		}
	}

	public int GenerateUnits()
	{
		allUnits = new List<FlockUnit>();
		locustCount = 0;
		for (int i = 0; i < flockSize; i++)
		{
			var randomVector = UnityEngine.Random.insideUnitSphere;
			if (randomVector.y < 0)
				randomVector.y = -randomVector.y;
			randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
			Vector3 randomSpawnPoint = GetRandomSpawnPoint();
			var spawnPosition = randomSpawnPoint + randomVector;
			var rotation = Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0);
			allUnits.Add(Instantiate(flockUnitPrefab, spawnPosition, rotation, this.transform));
			allUnits[i].AssignFlock(this);
			allUnits[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
			
			allUnits[i].SetDirectionsToCheckWhenAvoidingObstacles(spawnPoints);

			locustCount++;
		}

		return locustCount;
	}

	public void RemoveFlockUnit(FlockUnit flockUnit)
	{
		allUnits.Remove(flockUnit);
	}

	private Vector3 GetRandomSpawnPoint()
	{
		int index = UnityEngine.Random.Range(0, spawnPoints.Count);

		return spawnPoints[index];
	}
}
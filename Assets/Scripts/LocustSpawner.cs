using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocustSpawner : MonoBehaviour
{
/*
	[Header("Spawn Setup")]
	[SerializeField] private FlockUnit flockUnitPrefab;
	[SerializeField] private int flockSize;
	[SerializeField] private Vector3 spawnBound;

	[Header("Speed variables")]
	[Range(0, 10)]
	[SerializeField] private float _minSpeed;
	public float minSpeed { get { return _minSpeed; } }
	[Range(0, 10)]
	[SerializeField] private float _maxSpeed;
	public float maxSpeed { get { return _maxSpeed; } }

	[Header("Detection distances")]
	[Range(5, 20)]
	[SerializeField] private float _cohesionDistance;
	public float cohesionDistance { get { return _cohesionDistance; } }
	[Range(5, 20)]
	[SerializeField] private float _avoidanceDistance;
	public float avoidanceDistance { get { return _avoidanceDistance; } }
	[Range(5, 20)]
	[SerializeField] private float _alignmentDistance;
	public float alignmentDistance { get { return _alignmentDistance; } }

	[Header("Behavior weights")]
	[Range(0, 10)]
	[SerializeField] private float _cohesionWeight;
	public float cohesionWeight { get { return _cohesionWeight; } }
	[Range(0, 10)]
	[SerializeField] private float _avoidanceWeight;
	public float avoidanceWeight { get { return _avoidanceWeight; } }
	[Range(0, 10)]
	[SerializeField] private float _alignmentWeight;
	public float alignmentWeight { get { return _alignmentWeight; } }


	[Header("SpawnPoints")]
	[SerializeField] private List<Vector3> spawnPoints;

	public FlockUnit[] allUnits { get; set; }

	private List<FlockUnit> unitPool;

	void Start()
	{
		GenerateUnits();
	}

	void Update()
	{
		for (int i = 0; i < allUnits.Length; i++)
		{
			allUnits[i].MoveUnit();
		}
	}


	private void GenerateUnits()
	{
		allUnits = new FlockUnit[flockSize];
		Vector3 randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
		Vector3 direction = new Vector3(-1 * randomSpawnPoint.x, 0, -1 * randomSpawnPoint.z);
		for (int i = 0; i < flockSize; i++)
		{
			var randomVector = UnityEngine.Random.insideUnitSphere * 3.0f;
			var randomPosition = new Vector3(randomVector.x + spawnBound.x, randomVector.y + spawnBound.y, randomVector.z + spawnBound.z);
			var spawnPosition = randomSpawnPoint + randomPosition;

			var rotation = Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0);
			//rotation = Quaternion.LookRotation(direction, Vector3.up);

			allUnits[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation, this.transform);
			//allUnits[i].AssignFlock(this);

			allUnits[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
		}
	}
	*/
}

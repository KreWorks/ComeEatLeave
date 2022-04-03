using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public int width = 10;
	public int height = 10;
	public float cellSize = 2.0f;

	public GameObject fieldPrefab;
	public Transform plantParent;

	public UiController uiController;
	public Flock flock;

	public float spawnTime = 5.0f;

	protected Grid<Cell> grid;

	float allHealth;
	int cornCount = 0;
	int locustCount = 0;
	float timer;



    // Start is called before the first frame update
    void Awake()
    {
		timer = spawnTime;
		allHealth = 0;
		cornCount = 0;
		grid = new Grid<Cell>(width, height, cellSize, Vector3.zero);
		//grid.DrawLines();
		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				Vector3 position = grid.GetWorldPosition(x, y);
				
				GameObject field = Instantiate(fieldPrefab, position, Quaternion.identity, plantParent);
				Transform[] corns = field.GetComponentsInChildren<Transform>();
				for(int i = 0; i < corns.Length; i++) 
				{
					if (corns[i].tag == "Corn")
					{
						int rotationAngle = UnityEngine.Random.Range(0, 360);
						Quaternion rotation = Quaternion.Euler(corns[i].transform.rotation.eulerAngles.x, rotationAngle, 0);
						corns[i].rotation = rotation;
						allHealth += 100;
						cornCount++;

						CornController cc = corns[i].GetComponent<CornController>();
						cc.AddListenerOnHealthChange(HealthChange);
						cc.AddListenerOnPlantDeath(DecreaseCornCount);
					}
					
				}
				Cell cell = new Cell(x,y,grid, field);
			}
		}

		uiController.SetMaxHealth(allHealth);
		SetCornCount();
	}

	public void Update()
	{
		if (locustCount <= 0)
		{
			timer -= Time.deltaTime;

			uiController.SetLocustTimer(timer);
			if (timer <= 0)
			{
				locustCount = flock.GenerateUnits();
				uiController.SetLocustCounter(locustCount);
				timer = spawnTime;
			}
		}
	}

	void HealthChange(float change)
	{
		allHealth += change;
		uiController.ChangeHealth(change);
	}

	void DecreaseCornCount()
	{
		cornCount--;
		SetCornCount();
	}

	void SetCornCount()
	{
		uiController.SetCornCounter(cornCount);
	}

	public void DecreaseLocust()
	{
		locustCount--;
		uiController.SetLocustCounter(locustCount);
	}

}

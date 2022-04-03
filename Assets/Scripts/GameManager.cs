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

	protected Grid<Cell> grid;
	float allHealth;

    // Start is called before the first frame update
    void Awake()
    {
		allHealth = 0;
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

						CornController cc = corns[i].GetComponent<CornController>();
						cc.AddListenerOnHealthChange(HealthChange);
					}
					
				}
				Cell cell = new Cell(x,y,grid, field);
			}
		}
		Debug.Log("AllHealt in GameManager: " + allHealth);
		uiController.SetMaxHealth(allHealth);
	}

	void HealthChange(float change)
	{
		allHealth += change;
		uiController.ChangeHealth(change);
	}

}

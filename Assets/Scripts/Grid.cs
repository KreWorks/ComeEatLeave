using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridType>
{
	private int width;
	private int height;
	private float cellSize;
	private Vector3 originPosition;

	private TGridType[,] gridArray;

	public Grid(int width, int height, float cellSize, Vector3 originPosition)
	{
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;
		this.originPosition = originPosition;

		gridArray = new TGridType[width, height];
	}

	public void DrawLines()
	{
		for (int x = 0; x < gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < gridArray.GetLength(1); y++)
			{
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
			}
		}

		Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
		Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
	}

	public int GetWidth()
	{
		return width;
	}

	public int GetHeight()
	{
		return height;
	}

	public float GetCellSize()
	{
		return cellSize;
	}

	public Vector3 GetWorldPosition(int x, int y)
	{
		return (new Vector3(x, 0, y) + new Vector3(0.5f, 0, 0.5f) - new Vector3(width, 0, height) / 2.0f) * cellSize + originPosition;
	}

	private void GetXY(Vector3 worldPosition, out int x, out int y)
	{
		x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
		y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
	}

	public void SetValue(int x, int y, TGridType value)
	{
		if (x >= 0 && x < width && y >= 0 && y < height)
		{
			gridArray[x, y] = value;
		}
	}

	public void SetValue(Vector3 worldPosition, TGridType value)
	{
		int x, y;
		GetXY(worldPosition, out x, out y);
		SetValue(x, y, value);
	}

	public TGridType GetValue(int x, int y)
	{
		if (x >= 0 && x < width && y >= 0 && y < height)
		{
			return gridArray[x, y];
		}
		else
		{
			return default(TGridType);
		}
	}

	public TGridType GetValue(Vector3 worldPosition)
	{
		int x, y;
		GetXY(worldPosition, out x, out y);

		return GetValue(x, y);
	}
}

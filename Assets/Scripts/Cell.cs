using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public int x;
	public int y;
	public Grid<Cell> grid;
	public GameObject cellObject;

	public Cell(int x, int y, Grid<Cell> grid, GameObject cellObject)
	{
		this.x = x;
		this.y = y;
		this.grid = grid;
		this.cellObject = cellObject;
	}
}

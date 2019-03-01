using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData	{

	public int chunkNumber;
	public string worldType;
    public float cellArrayWidth;
    public float cellArrayHeight;

    public WorldData(cell Cell, float width, float height)
	{
		chunkNumber = Cell.getPosInArray();

		worldType = Cell.getState().ToString();

        cellArrayHeight = height;
        cellArrayWidth = width;

	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData	{

	public int chunkNumber;
	public string worldType;

	public WorldData(cell Cell)
	{
		chunkNumber = Cell.getPosInArray();

		worldType = Cell.getState().ToString();
	}
	
}

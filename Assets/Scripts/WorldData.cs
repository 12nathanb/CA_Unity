using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData	{

	public int[] chunkNumber;
	public string[] worldType;
	public float[] worldHeight;

	public WorldData(cell[,] Cell, int width, int height)
	{
        chunkNumber = new int[width * height];
        worldType = new string[width * height];
        worldHeight = new float[width * height];

       int counter = 0;
        for (int w = 0; w < width; w++)
        {
            for(int h = 0; h < height; h++)
            {
                chunkNumber[counter] = Cell[w, h].getPosInArray();
                worldType[counter] = Cell[w, h].getState().ToString();
                worldHeight[counter] = Cell[w, h].getWorldHeight();

                counter++;
            }
        }
	
	}
	
}

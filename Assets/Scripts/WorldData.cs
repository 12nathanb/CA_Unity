using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData	{

	public int[] chunkNumber;
	public string[] worldType;
	public float[] worldHeight;

    public int width;

    public int height;

	public WorldData(cell[,] Cell, int widthtemp, int heighttemp)
	{
        chunkNumber = new int[widthtemp * heighttemp];
        worldType = new string[widthtemp * heighttemp];
        worldHeight = new float[widthtemp * heighttemp];
        width = widthtemp;
        height = heighttemp;
        
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

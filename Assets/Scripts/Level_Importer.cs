using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Importer : MonoBehaviour {

	public cell[,] cellMap;
	public GameObject[,] cellMapObjects;
	private int width, height;

	public GameObject cellPrefab;

	public string FileName;

	// Use this for initialization
	void Start () 
	{
		generateMap();
		Import();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	  public void generateMap()
    {      
        cellMap = new cell[width, height];

        cellMapObjects = new GameObject[width, height];

        int counter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                cellMapObjects[x, z] = (GameObject)Instantiate(cellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                cellMap[x, z] = cellMapObjects[x, z].GetComponent<cell>();
                cellMap[x, z].setPosInArray(counter);
                counter++;
            }
        }

    }

    
	 public void Import()
    {
        int count = 0;
        WorldData data = Importer.LoadWorld(FileName);
        Debug.Log(data.worldType[count]);
        width = data.width;
        height = data.height;

        generateMap();

        for (int x = 0; x < width; x++)
        {
           for (int z = 0; z < height; z++)
           {
                Debug.Log(data.worldType[count]);

               if(data.worldType[count] == "grass")
               {
                    cellMap[x,z].setState(cellType.grass);
               }
               else if(data.worldType[count] == "water")
               {
                   cellMap[x,z].setState(cellType.water);
               }
               else if(data.worldType[count] == "darkWater")
               {
                   cellMap[x,z].setState(cellType.darkWater);
               }
               else if(data.worldType[count] == "sand")
               {
                   cellMap[x,z].setState(cellType.sand);
               }
                  cellMap[x,z].setWorldHeight(data.worldHeight[count]);
                cellMap[x,z].SelectedUpdate();

                count++;
           }

        }
        
    }
}

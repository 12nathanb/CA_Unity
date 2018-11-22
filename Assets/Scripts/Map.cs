using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

   public int width, height;

    public List<bool> cellMap;
    public List<GameObject> CellCollection;
    private GameObject block;
    public GameObject floor;
    TextMesh text;
	// Use this for initialization
	void Start ()
    {
      
        generateMap();
        DrawMap();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckPoint();
        }
       

        for (int c = 0; c < cellMap.Count; c++)
        {
            if (cellMap[c] == false)
            {
                CellCollection[c].GetComponent<Renderer>().material.color = Color.red;
            }

            if (cellMap[c] == true)
            {
                CellCollection[c].GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void generateMap()
    {
        

        for (int i = 0; i < width; i++)
        {
            for(int x = 0; x < height; x++)
            {
                cellMap.Add( RandomBool());
            }
        }
    }

    void CheckMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < height; x++)
            {

            }
        }
    }
    void DrawMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < height; x++)
            {
                if (cellMap[i * x] == true)
                {
                   CellCollection.Add( Instantiate(floor, new Vector3(i * 1, 0, x * 1), Quaternion.identity));
                    
                }
                else
                {
                    CellCollection.Add(Instantiate(floor, new Vector3(i * 1, 0, x * 1), Quaternion.identity));  
                }
               

            }
        }

        for (int c = 0; c < cellMap.Count; c++)
        {
            if (cellMap[c] == false)
            {
                CellCollection[c].GetComponent<Renderer>().material.color = Color.red;
            }
            text = CellCollection[c].GetComponentInChildren<TextMesh>();
            text.text = c.ToString();
        }

    }

    void CheckPoint()
    {
        for (int c = 0; c < cellMap.Count; c++)
        {
            int counter = 0;

            if (cellMap[c] == cellMap[c + 1])
            {
                counter++; //checks point above
            }

            if(cellMap[c] == cellMap[c + width])
            {
                counter++; //check point to the right
            }

            if (cellMap[c] == cellMap[c + (width + 1)])
            {
                counter++; //checks the point to the top right
            }

            if (cellMap[c] == cellMap[c + (width - 1)])
            {
                counter++; //check the poimt to the bottom right
            }

            if (cellMap[c] == cellMap[c - 1])
            {
                counter++; //check the point to the bottom 
            }

            if (cellMap[c] == cellMap[c  - width])
            {
                counter++;
            }

            if (cellMap[c] == cellMap[c - (width + 1)])
            {
                counter++;
            }

            if (cellMap[c] == cellMap[c - (width - 1)])
            {
                counter++;
            }

            if (counter > 4)
            {
                if (cellMap[c] == true)
                {
                    cellMap[c] = false;
                }

                if (cellMap[c] == false)
                {
                    cellMap[c] = true;
                }
            }
        }
    }

    bool RandomBool()
    {
        if (Random.value > 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}

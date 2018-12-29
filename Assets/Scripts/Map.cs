using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

   public int width, height;

    [Range(0f, 1.0f)]
    public float fillAmount;

    public GameObject[,] cellMap;
    public List<bool> bools;
    public GameObject[,] cellMap2;
    public int num;
    public GameObject floor;
    public int neighbours;
    TextMesh text;
   public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    // Use this for initialization
    void Start ()
    {
        cellMap = new GameObject[width, height];
        cellMap2 = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < height; x++)
            {
                cellMap[i,x] = (Instantiate(floor, new Vector3(i * 1, 0, x * 1), Quaternion.identity));
                cellMap2 = cellMap;
               
            }
        }

       generateMap();
        //DrawMap();
	}

    void generateMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < height; x++)
            {

                if (RandomBool() == true)
                {
                    bools.Add(true);
                   // cellMap[i, x] = (Instantiate(floor, new Vector3(i * 1, 0, x * 1), Quaternion.identity));
                    cellMap[i, x].gameObject.GetComponent<cellManager>().setState(cellStateforfloor.Alive);
                }
                else
                {
                    bools.Add(false);
                    //cellMap[i, x] = (Instantiate(floor, new Vector3(i * 1, 0, x * 1), Quaternion.identity));
                    cellMap[i, x].gameObject.GetComponent<cellManager>().setState(cellStateforfloor.Dead);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.A))
        {
            int count = 0;

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    int neighbours = 0;

                    neighbours = GetSurroundingWallCount(w, h);
                    cellMap[w, h].GetComponent<cellManager>().SetNeighbours(neighbours);
                    Debug.Log(w*h + " " + neighbours);

                    if (neighbours <= 3)
                    {
                        cellMap2[w, h].GetComponent<cellManager>().setState(cellStateforfloor.Dead);
                    }

                    if (neighbours >= 5)
                    {
                        cellMap2[w, h].GetComponent<cellManager>().setState(cellStateforfloor.Alive);
                    }
                    else
                    {
                       
                    }

                    text = cellMap[w, h].GetComponentInChildren<TextMesh>();
            
                    text.text = neighbours.ToString();

                    count++;
                   
                }
            }

            

           cellMap = cellMap2;
            


       }
    }

    

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        //check 8 tiles surrounding current one, however this could be changed depending on desired effect
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //horiz
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            { //vert
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                { //stay within map bounds
                    if (neighbourX != gridX || neighbourY != gridY)
                    { //don't consider tile we're looking at
                        if (cellMap[neighbourX, neighbourY].GetComponent<cellManager>().GetState() == cellStateforfloor.Alive)
                        {
                            wallCount++;
                        }
                        //if it's a wall, increase
                    }
                }
                
            }
        }
        return wallCount;
    }
    

    //void DrawMap()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int x = 0; x < height; x++)
    //        {
    //            if (cellMap[i, x].GetComponent<cellManager>().status == cellStateforfloor.Alive)
    //            {
                   
                    
    //            }
    //            else
    //            {
    //                CellCollection.Add(Instantiate(floor, new Vector3(i * 1, 0, x * 1), Quaternion.identity));  
    //            }
               

    //        }
    //    }

        //for (int i = 0; i < cellMap.Length; i++)
        //{
        //    text = CellCollection[i].GetComponentInChildren<TextMesh>();
        //    CellCollection[i].gameObject.name = i + " cube";
        //    text.text = i.ToString();
        //}
        //int count = 0;
        //for (int w= 0; w < width; w++)
        //{
        //    for (int h = 0; h < height; h++)
        //    {
        //        text = CellCollection[w * h].GetComponentInChildren<TextMesh>();
        //        CellCollection[w * h].gameObject.name = count + " cube";
        //        text.text = count.ToString();
        //        Debug.Log(count);
        //        count++;
                
        //    }
        //}
    //}

  
    bool RandomBool()
    {
        if (Random.value > fillAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    bool Rules (bool a, bool b, bool c)
    {
        if (a == true && b == true && c == true) return true;
            else if (a == true && b == true && c == false) return true;
            else if (a == true && b == false && c == true) return true;
      
        else if (a == true && b == false && c == false)
        {
            return false;
        }

        else if (a == false && b == true && c == true)
        {
            return true;
        }

        else if (a == false && b == true && c == false)
        {
            return false;
        }
        else if (a == false && b == false && c == true)
        {
            return false;
        }
        else if (a == false && b == false && c == false)
        {
            return false;
        }

        return false;
    }

    void AddNewLayer()
    {
        for (int c = 0; c < cellMap.Length; c++)
        {
            //if (cellMap[c] == false)
            //{

            //}
        }
    }
}

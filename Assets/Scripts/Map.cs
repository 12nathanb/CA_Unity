using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Map : MonoBehaviour {

   public int width, height, depth;


    public float fillAmount;
   
    public GameObject[,,] cellMap;
    public GameObject[,,] cellMap2;
    public int num;
    public GameObject floor;
    TextMesh text;

    public List<Vector3> newVerts;
    List <int>newTriangle;

    public Slider mainSlider;
    public Text widthField;
    public Text heightField;

    public void generateMap()
    {

        cellMap = new GameObject[width, height, depth];
        cellMap2 = new GameObject[width, height, depth];

        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < height; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    cellMap[i, x,z] = (Instantiate(floor, new Vector3(i * 1, z * 1, x * 1), Quaternion.identity));
                    //cellMap[i, x].transform.SetParent(this.transform);
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < height; x++)
            {
                for (int z = 0; z < depth; z++)
                {

                    if (RandomBool() == true)
                    {
                        cellMap[i, x, z].gameObject.GetComponent<cellManager>().setState(cellStateforfloor.Alive);
                    }
                    else
                    {
                        cellMap[i, x, z].gameObject.GetComponent<cellManager>().setState(cellStateforfloor.Dead);
                    }
                }
            }
        }

        cellMap2 = cellMap;
    }

    public void CreateMesh()
    {
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        //meshGen.GenerateMesh(cellMap, height, width, 1);
    }




    void Progress()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int z = 0; z < depth; z++)
                {
                    int neighbours = 0;

                    neighbours = GetSurroundingWallCount(w, h, z);
                    cellMap[w, h, z].GetComponent<cellManager>().SetNeighbours(neighbours);
                    Debug.Log(w * h + " " + neighbours);

                    if (neighbours <= 12)
                    {
                        cellMap2[w, h, z].GetComponent<cellManager>().setState(cellStateforfloor.Dead);
                        

                    }

                    if (neighbours >= 15)
                    {
                        cellMap2[w, h, z].GetComponent<cellManager>().setState(cellStateforfloor.Alive);
                    }
                    else
                    {

                    }

                    text = cellMap[w, h, z].GetComponentInChildren<TextMesh>();

                    text.text = neighbours.ToString();

                }
            }
        }

        cellMap = cellMap2;
    }
    // Update is called once per frame
    void Update()
    {

        int.TryParse(heightField.text.ToString(), out height);
        int.TryParse(widthField.text.ToString(), out width);
        fillAmount = mainSlider.value;
    }

    public void delete()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Destroy(cellMap[w, h, z]);
                    Destroy(cellMap2[w, h, z]);
                }
            }
        }
    }

    public void OnButtonClick()
    {
        Progress();
        CreateMesh();
    }
    
    public void Clear()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Destroy(cellMap[w, h, z]);
                    Destroy(cellMap2[w, h, z]);
                }
            }

        }
                
    }

    int GetSurroundingWallCount(int gridX, int gridY, int gridZ)
    {
        int wallCount = 0;
        //check 8 tiles surrounding current one, however this could be changed depending on desired effect
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //horiz
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            { //vert
                for (int neighbourZ = gridZ - 1; neighbourZ <= gridZ + 1; neighbourZ++)
                {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height && neighbourZ >= 0 && neighbourZ < depth)
                    { //stay within map bounds
                        if (neighbourX != gridX || neighbourY != gridY || neighbourZ != gridZ)
                        { //don't consider tile we're looking at
                            if (cellMap[neighbourX, neighbourY, neighbourZ].GetComponent<cellManager>().GetState() == cellStateforfloor.Alive)
                            {
                                wallCount++;
                            }
                            //if it's a wall, increase
                        }
                    }
                }
                
            }
        }
        return wallCount;
    }

  
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

}

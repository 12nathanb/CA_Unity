using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Map : MonoBehaviour {

    public int width, height;


    public float fillAmount;

    public cell[,] cellMap;

    List<string> TextureNames;

    public Slider mainSlider;

    public Text widthField;
    public Text heightField;
    public Text refineAmountText;
    public Text seedText;

    public Dropdown texDrop;

    public Toggle useRandomSeed;

    public List<Material> textures;

    public int refineAmount;
    public string seed;
    public bool randomSeed;
    float spaceOfTerrain;

    bool playRefine = false;

    int[,] borderOfTheLevel;

    public Text countDown;

    void Start()
    {
        
    }

    public void generateMap()
    {

        cellMap = new cell[width, height];
        

        RandomLevelgen();

        for(int i = 0; i < refineAmount; i++)
        {
            Progress();
        }



        //CreateBorder();
        //CreateMesh(borderOfTheLevel);
        create3Dworld(cellMap);
    }

    void CreateBorder()
    {
        int border = 1;

        borderOfTheLevel = new int[width + border * 2, height + border * 2];

        for (int x = 0; x < borderOfTheLevel.GetLength(0); x++)
        {

            for (int z = 0; z < borderOfTheLevel.GetLength(1); z++)
            {

                if (x >= border && x < width + border && z >= border && z < height + border)
                {

                    //borderOfTheLevel[x, z] = cellMap[x - border, z - border];

                }
                else
                {

                    borderOfTheLevel[x, z] = 1;

                }

            }

        }
    }

    void RandomLevelgen()
    {
        if (randomSeed == true)
        {
            seed = Time.time.ToString();
        }

        System.Random randomSeedGenerator = new System.Random(seed.GetHashCode());

      

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {

                cellMap[x, z].createCells();

                if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                {
                    cellMap[x, z].setState(cellType.grass);
                }
                else
                {
                    cellMap[x, z].setStateFromInt((randomSeedGenerator.Next(0, 100) < spaceOfTerrain) ? 1 : 0);
                }

                

                
            }
        }

    }

    public void create3Dworld(cell[,] Cell)
    {
        GameObject[,] world;

        world = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (Cell[x, z].getState() == cellType.grass)
                {
                    world[x, z] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    world[x, z].transform.position = new Vector3(x * 1,0, z * 1);
                    world[x, z].gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }

                if (Cell[x, z].getState() == cellType.water)
                {
                    
                    world[x, z].transform.position = new Vector3(x * 1, 0, z * 1);
                    world[x, z].transform.localScale = new Vector3(1, 0.5f, 1);
                    world[x, z].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
        }
    }

    public void CreateMesh(int[,] cell)
    {
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(cell, 1);
    }




    void Progress()
    {
        int counter = width * height;

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int neighbours = GetSurroundingWallCount(w, h);
                

                if (neighbours > 4)
                {
                    cellMap[w, h].setState(cellType.grass);
                }

                if (neighbours < 4)
                {
                    cellMap[w, h].setState(cellType.water);
                }
                counter--;
               


            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        int.TryParse(heightField.text.ToString(), out height);
        int.TryParse(widthField.text.ToString(), out width);
        int.TryParse(refineAmountText.text.ToString(), out refineAmount);
        seed = seedText.text.ToString();
        randomSeed = useRandomSeed;
        spaceOfTerrain = mainSlider.value;

        if(playRefine == true)
        {
            Progress();
            CreateBorder();
            CreateMesh(borderOfTheLevel);
        }
    }

    

        public void Play()
    {
      
            playRefine = true;
       

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
                        wallCount += cellMap[neighbourX, neighbourY].getStateInt();
                        //if it's a wall, increase
                    }
                    
                }
                else
                {
                    wallCount++;
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

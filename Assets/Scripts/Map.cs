using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Map : MonoBehaviour {

    public int width, height;


    public float fillAmount;

    public cell[,] cellMap;
    public GameObject[,] cellMapObjects;
    public GameObject cellPrefab;
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

        cellMapObjects = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
             cellMapObjects[x, z] = (GameObject)Instantiate(cellPrefab, new Vector3(x, 0, z), Quaternion.identity);
             cellMap[x, z] = cellMapObjects[x, z].GetComponent<cell>();
             cellMap[x, z].createCells();
             
            }
        }

    
        RandomLevelgen();

        for(int i = 0; i < refineAmount; i++)
       {
      Progress();
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
                if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                {
                    cellMap[x, z].setState(cellType.water);
                     cellMap[x, z].SelectedUpdate();                
                }
                else
                {
                    cellMap[x, z].setStateFromInt((randomSeedGenerator.Next(0, 100) < spaceOfTerrain) ? 1 : 0);
                     cellMap[x, z].SelectedUpdate();
                }

               

                
            }
        }

    }




    void Progress()
    {
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
                
               
                 cellMap[w, h].SelectedUpdate();

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

        }
    }

    

        public void Play()
    {
      
            playRefine = true;
       

    }

   

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        int sandCount = 0;
        int waterCount = 0;
        int dirtCount = 0;
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

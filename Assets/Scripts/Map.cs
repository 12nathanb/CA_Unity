using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Map : MonoBehaviour {

    public int width, height;


    public float fillAmount;

    public cell[,] cellMap;
    public GameObject[,] cellMapObjects;
    public GameObject cellPrefab;
    List<string> TextureNames;

    public Slider mainSlider;
    public Dropdown dropdown;

    public InputField heightInput;
    public InputField widthInput;
    public InputField seedInput;
    int dropDownValue;
    public Text widthField;
    public Text heightField;
    public Text refineAmountText;
    public Text seedText;

    public Toggle useRandomSeed;

    public int refineAmount;
    public string seed;
    public bool randomSeed;
    float spaceOfTerrain;

    bool playRefine = false;

    void Start()
    {
      
    }

    public void generateMap()
    {
        setGridSize();

        instansiateArrays();



        int counter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
             cellMapObjects[x, z] = (GameObject)Instantiate(cellPrefab, new Vector3(x, 0, z), Quaternion.identity);
             cellMap[x, z] = cellMapObjects[x, z].GetComponent<cell>();
             cellMap[x, z].createCells();
             cellMap[x, z].setPosInArray(counter);
             counter++;
            }
        }

    
        RandomLevelgen();
    }

    void instansiateArrays()
    {
        cellMap = new cell[width, height];

        cellMapObjects = new GameObject[width, height];
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
                    cellMap[w, h].setState(cellType.water);
                }

                if (neighbours < 4)
                {
                    cellMap[w, h].setState(cellType.grass);
                }
                
               
                 cellMap[w, h].SelectedUpdate();

            }
        }


    }
    // Update is called once per frame
    void Update()
    {   
        if (dropdown.value != 10)
        {
            heightInput.image.color = Color.black;
            widthInput.image.color = Color.black;
            heightField.gameObject.SetActive(false);
            widthField.gameObject.SetActive(false);
        }
        else
        {
            heightField.gameObject.SetActive(true);
            widthField.gameObject.SetActive(true);
            heightInput.image.color = Color.white;
            widthInput.image.color = Color.white;
        }

    

        int.TryParse(refineAmountText.text.ToString(), out refineAmount);

        randomSeed = true;

        if(useRandomSeed.isOn)
        {      
            randomSeed = true;
            seedInput.image.color = Color.black;
        }
        else 
        {
            seed = seedText.text.ToString();
            randomSeed = false;
            seedInput.image.color = Color.white;
        }
        
        spaceOfTerrain = mainSlider.value;
        if(playRefine == true)
        {
            for(int i = 0; i < refineAmount; i++)
         {
            StartCoroutine(playProgress());
         }

         playRefine = false;

        }
    }


    void setGridSize()
    {
        if (dropdown.value == 0)
        {
            height = 10;
            width = 10;
            print("10x10");
        }
        else if (dropdown.value == 1)
        {
            height = 20;
            width = 20;
            print("20x20");
        }
        else if (dropdown.value == 2)
        {
            height = 30;
            width = 30;
            print("30x30");
        }
        else if (dropdown.value == 3)
        {
            height = 40;
            width = 40;
            print("40x40");
        }
        else if (dropdown.value == 4)
        {
            height = 50;
            width = 50;
            print("50x50");
        }
        else if (dropdown.value == 5)
        {
            height = 60;
            width = 60;
            print("60x60");
        }
        else if (dropdown.value == 6)
        {
            height = 70;
            width = 70;
            print("70x70");
        }
        else if (dropdown.value == 7)
        {
            height = 80;
            width = 80;
            print("80x80");
        }
        else if (dropdown.value == 8)
        {
            height = 90;
            width = 90;
            print("90x90");
        }
        else if (dropdown.value == 9)
        {
            height = 100;
            width = 100;
            print("100x100");
        }
        else if (dropdown.value == 10)
        {
            int.TryParse(heightField.text.ToString(), out height);
            int.TryParse(widthField.text.ToString(), out width);
            print("Custom");
        }

       
       
    }

    IEnumerator playProgress()
    {       
        yield return new WaitForSeconds(1f);
        Progress();
    }

    
        public void Play()
        {
        if (playRefine == false)
        {
            playRefine = true;
        }
         else
        {
          playRefine = false;
         } 
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
                       
                        if (cellMap[neighbourX, neighbourY].getState() == cellType.grass)
                        {
                            dirtCount++;
                        }

                          if (cellMap[neighbourX, neighbourY].getState() == cellType.water)
                        {
                            waterCount++;
                        }
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

    public void Export()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Exporter.SaveWorld(cellMap[x,z], width, height);
            }

        }
    
    }

    public void Import()
    {
    

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                WorldData data = Importer.LoadWorld(cellMap[x,z].getPosInArray());


                cellMap[x,z].setPosInArray(data.chunkNumber);
                if(data.worldType == "grass")
                {
                     cellMap[x,z].setState(cellType.grass);
                }
                else if(data.worldType == "water")
                {
                    cellMap[x,z].setState(cellType.water);
                }
                cellMap[x,z].SelectedUpdate();
            }

        }
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

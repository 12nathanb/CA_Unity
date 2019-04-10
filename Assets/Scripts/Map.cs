using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.IO;
public class Map : MonoBehaviour {


    //---Private Variables---
    private int width, height;
    private int refineAmount;
    private bool deleted = true;
    private int waterCount = 0;

    private int sandCount = 0;

    private int grassCount = 0;
    private bool importing = false;

    //---public Variables---
    public cell[,] cellMap;
    public GameObject[,] cellMapObjects;
    public GameObject cellPrefab;
    public Slider mainSlider;
    public Dropdown dropdown;
    public Dropdown startingBlock;
    public InputField heightInput;
    public InputField widthInput;
    public InputField seedInput;
    public Text ruleAmountText;
    public Text saveName;
    public Text loadName;
    public Text widthField;
    public Text heightField;
    public Text refineAmountText;
    public Text seedText;
    public Toggle useRandomSeed;
    public Toggle world;
    public Toggle beaches;
    public RectTransform worldText;
    public string seed;
    public bool randomSeed;
    float spaceOfTerrain;
    public int ruleAmount;
    bool playRefine = false;
    float MaxHeight = 2;
    public bool worldCreate;
    public Rules[] ruleArray;


    void Start()
    {     
        CheckRuleFile();
        LoadRuleText();   
        world.isOn = true;
    }

    //This function checks to make sure the program has a rule file loaded 
    void CheckRuleFile()
    {
        string fileName = Application.persistentDataPath + "/Rules.txt";
        ruleArray = new Rules[ruleAmount];
        string ruleLocation =  Application.streamingAssetsPath + "/Rules.txt";
        Debug.Log(Application.streamingAssetsPath + "/Rules.txt");

        if(System.IO.File.Exists(fileName))
        {
            Debug.Log("Does Exist");
        }
        else
        {
            System.IO.File.Copy(ruleLocation, fileName, true);
         
        }
    }

    //This function is incharge of generating the map
    public void generateMap()
    {
        if(deleted== true)
        {
            deleted = false;

            if(importing == false)
            {
                setGridSize();
            }
            
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

            if(importing == false)
            {
                RandomLevelgen();
            }
        }
    }

    //this function is in charge of creating the inital random map
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
                
                int num = ((randomSeedGenerator.Next(0, 100) < spaceOfTerrain) ? 1 : 0);

                if(num == 0)
                {
                    if(startingBlock.value == 0)
                    {
                        cellMap[x, z].setState(cellType.grass);
                    }
                    else if(startingBlock.value == 1)
                    {
                        cellMap[x, z].setState(cellType.sand);
                    }
                }
                else
                {
                    cellMap[x, z].setState(cellType.water);
                }
                
                       
                
                if (cellMap[x, z].getState() == cellType.grass || cellMap[x, z].getState() == cellType.sand)
                {
                    cellMap[x, z].height = Random.Range(1f, MaxHeight);
                    cellMap[x, z].SelectedUpdate();
                }
                else
                {
                    cellMap[x, z].height = 1;
                }
               
                cellMap[x, z].SelectedUpdate();
                
            }
        }

    }

    //This function is used to set the cells state by passing data through
    void setCellState(cell[,] array, int width, int height, string state)
    {
        if(state == "grass")
        {
            array[width, height].setState(cellType.grass);
        }
         if(state == "water")
        {
            array[width, height].setState(cellType.water);
        }
        if(state == "sand")
        {
            array[width, height].setState(cellType.sand);
        }
        if(state == "dark water")
        {
            array[width, height].setState(cellType.darkWater);
        }
    }

    //This function itterates the CA through its stages
    void Progress()
    {
        if(world.isOn == true)
        {
            worldCreate = true;
        }
        else
        {
            worldCreate = false;
        }

        cell[,] tempCellArray;
        tempCellArray = cellMap;

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                
                 int neighbours = GetSurroundingWallCount(w, h, cellMap[w, h].getState());

                for (int r = 0; r < ruleAmount; r++)
                {
                    
                    if(ruleArray[r].Operator == ">")
                    {
                        if (neighbours > ruleArray[r].Amount)
                        {
                            setCellState(tempCellArray, w,h,ruleArray[r].Output);
                        }
                    }
                    
                    if(ruleArray[r].Operator == "<")
                    {
                        if (neighbours < ruleArray[r].Amount)
                        {
                            setCellState(tempCellArray, w,h,ruleArray[r].Output);
                        }
                    }

                    if(ruleArray[r].Operator == "=")
                    {
                        if (neighbours == ruleArray[r].Amount)
                        {
                            setCellState(tempCellArray, w,h,ruleArray[r].Output);
                        }
                    }


                }

                if(worldCreate == true)
                {
                    if(beaches.isOn)
                    {
                        if (waterCount >= 3 && grassCount >=3)
                        {
                            setCellState(tempCellArray, w, h, "sand");
                        }

                    }
                    
                    if (waterCount >= 8)
                    {
                        setCellState(tempCellArray, w, h, "dark water");
                    }
                }
            }
        }

        UpdateCellMap(tempCellArray);
    }
    
    //This updates the maps cell states everytime it itterates
    void UpdateCellMap(cell[,] temp)
    {
        cellMap = temp;

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                cellMap[w,h].SelectedUpdate();
            }

        }
    }

    void Update()
    {   
        if (dropdown.value != 10)
        {
            heightInput.gameObject.SetActive(false);
            widthInput.gameObject.SetActive(false);
        }
        else
        {
            heightInput.gameObject.SetActive(true);
            widthInput.gameObject.SetActive(true);
        }

    

        int.TryParse(refineAmountText.text.ToString(), out refineAmount);
        int.TryParse(ruleAmountText.text.ToString(), out ruleAmount);
        randomSeed = true;

        if(world.isOn == true)
        {
            worldText.GetComponent<Text>().text = "World generation";
        }
        else
        {
            worldText.GetComponent<Text>().text = "Cellular Automata";
        }
        if(useRandomSeed.isOn)
        {      
            randomSeed = true;
            seedInput.gameObject.SetActive(false);
        }
        else 
        {
            seed = seedText.text.ToString();
            randomSeed = false;
            seedInput.gameObject.SetActive(true);
        }
        
        spaceOfTerrain = mainSlider.value;

        
    }


    void setGridSize()
    {
        if (dropdown.value == 0)
        {
            height = 10;
            width = 10;
        }
        else if (dropdown.value == 1)
        {
            height = 20;
            width = 20;
        }
        else if (dropdown.value == 2)
        {
            height = 30;
            width = 30;
        }
        else if (dropdown.value == 3)
        {
            height = 40;
            width = 40;
        }
        else if (dropdown.value == 4)
        {
            height = 50;
            width = 50;
        }
        else if (dropdown.value == 5)
        {
            height = 60;
            width = 60;
        }
        else if (dropdown.value == 6)
        {
            height = 70;
            width = 70;
        }
        else if (dropdown.value == 7)
        {
            height = 80;
            width = 80;
        }
        else if (dropdown.value == 8)
        {
            height = 90;
            width = 90;
        }
        else if (dropdown.value == 9)
        {
            height = 100;
            width = 100;
        }
        else if (dropdown.value == 10)
        {
            int.TryParse(heightField.text.ToString(), out height);
            int.TryParse(widthField.text.ToString(), out width);
        }

       
       
    }

    IEnumerator playProgress()
    {      
        if(playRefine == true)
        {
            for(int i = 0; i < refineAmount; i++)
            {
                Progress();
                yield return new WaitForSeconds(0.5f);
            }

            playRefine = false;

        }
    }

    
        public void Play()
        {
        if (playRefine == false)
        {
            playRefine = true;
            StartCoroutine(playProgress());
        }
         else
        {
          playRefine = false;
         } 
    }

   

    int GetSurroundingWallCount(int gridX, int gridY, cellType type)
    {
        int wallCount = 0;
        float sum = 0;
        int count = 0;
         waterCount = 0;

         sandCount = 0;

         grassCount = 0;

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
                    
                

                        if(cellMap[neighbourX, neighbourY].getState() == type)
                        {
                            count++;
                             sum += cellMap[neighbourX, neighbourY].height;
                             
                        }

                        if(worldCreate == true)
                        {
                            if(cellMap[neighbourX, neighbourY].getState() == cellType.grass)
                            {
                                grassCount++;
                            }
                            else if(cellMap[neighbourX, neighbourY].getState() == cellType.water)
                            {
                                waterCount++;
                            }
                            else if(cellMap[neighbourX, neighbourY].getState() == cellType.darkWater)
                            {
                                waterCount++;
                            }
                            else if(cellMap[neighbourX, neighbourY].getState() == cellType.sand)
                            {
                                sandCount++;
                            }
                        }
                    }
                }
                else
                {
                    wallCount++;
                }

            }
        }

        if(cellMap[gridX, gridY].getStateInt() == 1)
        {
            sum = sum / count;

            if(sum < 1)
            {
                sum = 1;
            } 

            if(sum != 0)
            {
                 cellMap[gridX, gridY].height = sum;
            }
           
        }
        
        if(worldCreate == true)
        {
            return wallCount;
        }
        else
        {
            return count;
        }
        
    }

    public void Export()
    {
        Exporter.SaveWorld(cellMap, width, height, saveName.text.ToString());
    }

    public void Import()
    {
        int count = 0;
        WorldData data = Importer.LoadWorld(loadName.text.ToString());
        Debug.Log(loadName.text.ToString());
        Debug.Log(data.worldType[count]);
        width = data.width;
        height = data.height;

        importing = true;
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

        importing = false;
        
    }


    public void DeleteMap()
    {
        if(deleted == false)
        {
            for (int tempw = 0; tempw < cellMap.GetLength(0); tempw++)
            {
                for (int temph = 0; temph < cellMap.GetLength(1); temph++)
                {
                    Destroy(cellMap[tempw, temph].gameObject); 
                }
            }

            deleted = true;
        }

        
    }

    public void LoadRuleText()
    {
       string text = null;
        string[] textArray;
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/Rules.txt");
        
        while(!sr.EndOfStream)
        {
           text += sr.ReadLine();
        }
        
        textArray = text.Split(char.Parse(" "));

        int temp = 0;
        string[] name = new string[ruleAmount];
        string[] op = new string[ruleAmount];
        int[] amount = new int[ruleAmount];
        string[] output = new string[ruleAmount];

        for (int i = 0; i < textArray.Length; i++)
        {
            for (int r = 0; r < ruleAmount; r++)
            {
                if(textArray[i] == "Rule")
                {
                    temp = int.Parse(textArray[i+1]);
                    name[temp] = "Rule " + temp.ToString();
                }
                
                if(textArray[i] == "operator")
                {
                  op[temp] = textArray[i+1];
                }
                if(textArray[i] == "amount")
                {
                    amount[temp] = int.Parse(textArray[i+1]);
                }
                if(textArray[i] == "output")
                {
                    output[temp] = textArray[i+1];
                }
                ruleArray[r] = new Rules(name[r], op[r], amount[r], output[r]);
            }    
            
        }
        
    }

}

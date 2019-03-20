using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.IO;
public class Map : MonoBehaviour {

    public int width, height;


    public float fillAmount;

    public cell[,] cellMap;
    public GameObject[,] cellMapObjects;
    public GameObject cellPrefab;

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
    public int ruleAmount;
    bool playRefine = false;
    public string[] textArray;

    public Rules[] ruleArray;
    void Start()
    {
        string fileName = Application.persistentDataPath + "/Rules.txt";
        ruleArray = new Rules[ruleAmount];

        if(System.IO.File.Exists(fileName))
        {
            
        }
        else
        {
            FileUtil.CopyFileOrDirectory("Assets/Rules.txt", fileName);
        }

        LoadRuleText();   
    }

    public void generateMap()
    {
        setGridSize();

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

    
        RandomLevelgen();
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
                
                int num = ((randomSeedGenerator.Next(0, 100) < spaceOfTerrain) ? 1 : 0);
                 cellMap[x, z].setStateFromInt(num);       
                
                if (cellMap[x, z].getState() == cellType.grass)
                {
                    cellMap[x, z].height = Random.Range(1f, 2f);
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

    void setCellState(int width, int height, string state)
    {
        if(state == "grass")
        {
            cellMap[width, height].setState(cellType.grass);
        }
         if(state == "water")
        {
            cellMap[width, height].setState(cellType.water);
        }
    }
    void Progress()
    {
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
                            setCellState(w,h,ruleArray[r].Output);
                            //tempCellArray[w, h].SelectedUpdate();
                        }
                    }
                    
                    if(ruleArray[r].Operator == "<")
                    {
                        if (neighbours < ruleArray[r].Amount)
                        {
                            setCellState(w,h,ruleArray[r].Output);
                            //tempCellArray[w, h].SelectedUpdate();
                        }
                    }

                    if(ruleArray[r].Operator == "=")
                    {
                        if (neighbours == ruleArray[r].Amount)
                        {
                            setCellState(w,h,ruleArray[r].Output);
                           //tempCellArray[w, h].SelectedUpdate();
                        }
                    }
                }
            }
        }

        UpdateCellMap(tempCellArray);
    }
    // Update is called once per frame

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
            print("30x30");
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

       cellMap = new cell[width, height];

     cellMapObjects = new GameObject[width, height];
       
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
        print(Time.time);
        
        print(Time.time);
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
            cellMap[gridX, gridY].height = sum;
        }
        
        
        return wallCount;
    }

    public void Export()
    {
       
        Exporter.SaveWorld(cellMap, width, height);
    
    }

    public void Import()
    {
        int count = 0;

        for (int x = 0; x < width; x++)
        {
           for (int z = 0; z < height; z++)
           {
               WorldData data = Importer.LoadWorld(cellMap[x,z].getPosInArray());
               cellMap[x,z].setPosInArray(data.chunkNumber[count]);
               if(data.worldType[count] == "grass")
               {
                    cellMap[x,z].setState(cellType.grass);
               }
               else if(data.worldType[count] == "water")
               {
                   cellMap[x,z].setState(cellType.water);
               }
               cellMap[x,z].setWorldHeight(data.worldHeight[count]);
               cellMap[x,z].SelectedUpdate();

               count++;
           }

        }
    }
  
    bool RandomBool()
    {
        if (Random.value > 0.5)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void LoadRuleText()
    {
       string text = null;

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

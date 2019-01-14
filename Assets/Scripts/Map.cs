using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Map : MonoBehaviour {

    public int width, height;


    public float fillAmount;

    public int[,] cellMap;
    public int[,] cellMap2;
    public int num;
    public GameObject floor;
    TextMesh text;

    public List<Vector3> newVerts;
    List<int> newTriangle;

    public Slider mainSlider;
    public Text widthField;
    public Text heightField;
    public int refineAmount;
    public string seed;
    public bool randomSeed;
    float spaceOfTerrain;
    public void generateMap()
    {

        cellMap = new int[width, height];
        cellMap2 = new int[width, height];

        RandomLevelgen();

        for(int i = 0; i < refineAmount; i++)
        {
            Progress();
        }

        int border = 1;

        int[,] borderOfTheLevel = new int[width + border * 2, height + border * 2];

        for (int x = 0; x < borderOfTheLevel.GetLength(0); x++)
        {

            for (int z = 0; z < borderOfTheLevel.GetLength(1); z++)
            {

                if (x >= border && x < width + border && z >= border && z < height + border)
                {

                    borderOfTheLevel[x, z] = cellMap[x - border, z - border];

                }
                else
                {

                    borderOfTheLevel[x, z] = 1;

                }

            }

        }
        //CreateMesh(cellMap);
        CreateMesh(borderOfTheLevel);
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
                    cellMap[x, z] = 1;
                }
                else
                {
                    cellMap[x, z] = (randomSeedGenerator.Next(0, 100) < spaceOfTerrain) ? 1 : 0;
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
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int neighbours = GetSurroundingWallCount(w, h);
                

                if (neighbours > 4)
                {
                    cellMap2[w, h] = 1;
                }

                if (neighbours < 4)
                {
                    cellMap2[w, h] = 0;
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
        spaceOfTerrain = mainSlider.value;
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
                        wallCount += cellMap[neighbourX, neighbourY];
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

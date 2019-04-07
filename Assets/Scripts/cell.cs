using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType {air, grass, water, sand};

public class cell : MonoBehaviour {

    public cellType status;

    int posIntArray;
    Vector3 Position;
    GameObject cellOBJ;
    public float height = 1;

    public bool Tree = false;
    public GameObject TreePrefab;

    public Material lightGreen;

    public Material darkGreen;

    public Material water;

    public Material sand;
    GameObject treeObj;
	
	// Update is called once per frame
	public void SelectedUpdate ()
    {
        if(height < 1)
        {
            height = 1;
        }
       if (status == cellType.air)
       {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
       }
       else if (status == cellType.grass)
       {
           if(height <= 1.5)
           {
               this.gameObject.GetComponent<MeshRenderer>().material = lightGreen;
           }
           else if(height >= 1.5)
           {
               this.gameObject.GetComponent<MeshRenderer>().material = darkGreen;
           }
           else
           {
                this.gameObject.GetComponent<MeshRenderer>().material = lightGreen;
           }
             this.gameObject.transform.localScale = new Vector3(1, height, 1 );
           

           
       }
       else if (status == cellType.water)
       {
           this.gameObject.transform.localScale = new Vector3(1, 0.8f, 1 );
           this.gameObject.GetComponent<MeshRenderer>().material = water;

       }
       else if (status == cellType.sand)
       {
           this.gameObject.transform.localScale = new Vector3(1, 1, 1 );
           this.gameObject.GetComponent<MeshRenderer>().material = sand;
       }

    //    if(Tree == true && status == cellType.grass)
    //        {
    //            treeObj = Instantiate(TreePrefab, new Vector3(this.transform.position.x, height, this.transform.position.z), Quaternion.identity);
    //        }
    //        else if (Tree == true && status != cellType.grass)
    //        {
    //            Destroy(treeObj);
    //        }


     
    }

    public void setState(cellType state)
    {
        status = state;
    }

    public cellType getState()
    {
        return status;
    }

    public int getStateInt()
    {
        if (status == cellType.grass)
        {
            return 0;
        }
        else
        {
            return 1;
        }

    }

    public void setStateFromInt(int type)
    {
       
        if (type == 0)
        {
            setState(cellType.grass);
        }
        else if (type == 1)
        {
            setState(cellType.water);
        }
        else if (type == 2)
        {
            setState(cellType.sand);
        }
        else
        {
            setState(cellType.air);
        }
    }

    public void setPosition(Vector3 pos)
    {
        Position = pos;
    }

   public Vector3 getPosition()
    {
        return Position;
    }

    public int getPosInArray()
    {
        return posIntArray;
    }

    public void setPosInArray(int temp)
    {
        posIntArray = temp;
    }

    public float getWorldHeight()
    {
        return height;
    }

    public void setWorldHeight(float h)
    {
        height = h;
    }
    
}

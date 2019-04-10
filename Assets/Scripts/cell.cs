using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType {air, grass, water, sand, darkWater};

public class cell : MonoBehaviour {

    private int posIntArray;
    private Vector3 Position;

    public cellType status;
    public float height = 1;
    public Material lightGreen;
    public Material darkGreen;
    public Material water;
    public Material darkWater;
    public Material sand;
	
	// Update is called once per frame
	public void SelectedUpdate ()
    {

        if(height < 1 || System.Single.IsNaN(height))
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
               // this.gameObject.GetComponent<MeshRenderer>().material = lightGreen;
           }
             this.gameObject.transform.localScale = new Vector3(1, height, 1 );
          

           
       }
       else if (status == cellType.water)
       {
           this.gameObject.transform.localScale = new Vector3(1, 0.8f, 1 );
           this.gameObject.GetComponent<MeshRenderer>().material = water;

       }

       else if (status == cellType.darkWater)
       {
           this.gameObject.transform.localScale = new Vector3(1, 0.8f, 1 );
           this.gameObject.GetComponent<MeshRenderer>().material = darkWater;

       }
       else if (status == cellType.sand)
       {
           this.gameObject.transform.localScale = new Vector3(1, 1, 1 );
           this.gameObject.GetComponent<MeshRenderer>().material = sand;
       } 
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
        if (status == cellType.grass || status == cellType.sand)
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

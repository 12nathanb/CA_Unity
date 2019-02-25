using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType {air, grass, water, sand};

public class cell : MonoBehaviour {

    public cellType status;
    Vector3 Position;
    GameObject cellOBJ;

    cellManager manager;

	// Use this for initialization
	void Start ()
    {
       manager = this.gameObject.GetComponent<cellManager>();
        
	}
	
    public void createCells()
    {
        //GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cellOBJ = temp;
        status = cellType.air;
    }
	// Update is called once per frame
	public void SelectedUpdate ()
    {

    if (status == cellType.air)
       {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
       }
       else if (status == cellType.grass)
       {
           this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
       }
       else if (status == cellType.water)
       {
           this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
       }
       else if (status == cellType.sand)
       {
           this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
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
        if (status == cellType.grass)
        {
            return 1;
        }
        else
        {
            return 0;
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
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType {air, grass, water, sand};

public class cell : MonoBehaviour {

    cellType status;
    Vector3 Position;
    GameObject cellOBJ;

	// Use this for initialization
	void Start ()
    {
       
        
	}
	
    public void createCells()
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cellOBJ = temp;
        setState(cellType.air);
    }
	// Update is called once per frame
	void Update ()
    {
        cellOBJ.transform.position = Position;

        if (status == cellType.air)
        {
            cellOBJ.SetActive(false);
        }
        else if (status == cellType.water)
        {
            cellOBJ.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            cellOBJ.SetActive(true);
        }
        else if (status == cellType.grass)
        {
            cellOBJ.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            cellOBJ.SetActive(true);
        }
        if (status == cellType.sand)
        {
            cellOBJ.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            cellOBJ.SetActive(true);
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
            setState(cellType.air);
        }
        else if (type == 1)
        {
            setState(cellType.grass);
        }
        else if (type == 2)
        {
            setState(cellType.water);
        }
        else if (type == 3)
        {
            setState(cellType.sand);
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

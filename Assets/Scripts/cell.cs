using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum cellType {air, grass, water, sand};

public class cell : MonoBehaviour {

    cellType status;
    Vector3 Position;
    GameObject cellOBJ;

	// Use this for initialization
	void Start ()
    {
        cellOBJ = GameObject.CreatePrimitive(PrimitiveType.Cube);
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

    void setState(cellType state)
    {
        status = state;
    }

    cellType getState()
    {
        return status;
    }

    void setPosition(Vector3 pos)
    {
        Position = pos;
    }

    Vector3 getPosition()
    {
        return Position;
    }
    
}

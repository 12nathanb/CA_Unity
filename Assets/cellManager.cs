﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellStateforfloor { Alive, Dead };

public class cellManager : MonoBehaviour {

    public cellStateforfloor status;
    Renderer Floorcolor;
    public bool alive;
    public int neighbourCount;
    public Texture grass;
    public Texture water;
	// Use this for initialization
	void Start ()
    {
        Floorcolor = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
		if (status == cellStateforfloor.Alive)
        {
            alive = true;
            Floorcolor.material.SetTexture("_MainTex", grass);
        }
        else
        {
            alive = false;
            Floorcolor.material.SetTexture("_MainTex", water);
        }
	}

    public void setState(cellStateforfloor state)
    {
        status = state;
    }

    public cellStateforfloor GetState()
    {
        return status;
    }

    public void SetNeighbours(int n)
    {
        neighbourCount = n;
    }
    //public void CurrentState(bool i)
    //{
    //    if (i == true)
    //    {
    //        status = cellStateforfloor.Alive;
    //    }

    //    else 
    //    {
    //        status = cellStateforfloor.Dead;
    //    }
    //}
}

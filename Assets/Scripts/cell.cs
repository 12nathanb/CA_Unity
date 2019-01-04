using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum cellState {Alive, Dead};

public class cell : MonoBehaviour {

    cellState status;
    Vector3 Position;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setState(cellState state)
    {
        status = state;
    }

    cellState getState()
    {
        return status;
    }

    
}

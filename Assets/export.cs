using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class export : MonoBehaviour {

    OBJExporter objExp;

	// Use this for initialization

    void Start()
    {
        objExp = new OBJExporter();
    }
	public void ExportOBJ () {
		
        objExp.Export("D:/New folder");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

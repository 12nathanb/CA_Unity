using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public Transform target;
    public Camera cam;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = this.transform.position;

        if (Input.GetKey(KeyCode.W))
        {

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, pos.z += 1);
        }

        if (Input.GetKey(KeyCode.S))
        {

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, pos.z -= 1);
        }

        if (Input.GetKey(KeyCode.A))
        {

            this.transform.position = new Vector3(pos.x -= 1, this.transform.position.y, pos.z);
        }

        if (Input.GetKey(KeyCode.D))
        {

            this.transform.position = new Vector3(pos.x+= 1, this.transform.position.y, pos.z);
        }

        if (Input.mouseScrollDelta.y > 0 && this.transform.position.y > 10)
        {
            float y = this.transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, pos.y -= 10, this.transform.position.z);
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            float y = this.transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, pos.y += 10, this.transform.position.z);
        }
    }
}

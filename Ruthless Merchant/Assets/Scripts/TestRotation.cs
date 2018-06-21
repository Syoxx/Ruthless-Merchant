using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{

    public GameObject center;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime);
        if(Input.GetKey(KeyCode.Q) && transform.rotation.eulerAngles.z < 180)
        {
            Debug.Log(transform.rotation.eulerAngles.z);
            transform.RotateAround(center.transform.position, new Vector3(0,0,20) , 450 * Time.deltaTime);
            transform.position += new Vector3(0,0.00055f,0);
        }

	    if (Input.GetKey(KeyCode.E) && transform.rotation.eulerAngles.z < 180)
	    {
	        Debug.Log(transform.rotation.eulerAngles.z);
	        transform.RotateAround(center.transform.position, new Vector3(0, 0, -20), 70 * Time.deltaTime);
	        transform.position += new Vector3(0, -0.00055f, 0);
	    }
    }
}

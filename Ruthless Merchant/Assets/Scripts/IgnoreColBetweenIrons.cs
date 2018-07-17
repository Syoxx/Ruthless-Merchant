using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColBetweenIrons : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

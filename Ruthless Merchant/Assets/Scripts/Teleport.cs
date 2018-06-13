using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform targetPos;
    private void OnTriggerEnter(Collider other)
    {
        
        other.transform.position = targetPos.position;
        other.transform.position += new Vector3(0, 1);

    }
}

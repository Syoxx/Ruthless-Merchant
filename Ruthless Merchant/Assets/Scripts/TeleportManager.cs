using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour {

    //By Daniil Masliy
    [SerializeField] [Tooltip("Drag a GameObject there. This object will be considered as Destination")] private GameObject TeleportDestination;

    Rigidbody rb;
    [HideInInspector]
    public Vector3 positionOfDestination;

    /// <summary>
    /// Use this position, to 
    /// </summary>
    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
    public void Teleport()
    {
        positionOfDestination = TeleportDestination.transform.position;
        //ThingToTeleportate.transform.position = positionOfDestination;
        rb.transform.position = positionOfDestination;

    }
}

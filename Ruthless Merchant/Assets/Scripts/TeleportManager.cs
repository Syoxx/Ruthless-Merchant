using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour {

    //By Daniil Masliy
    [SerializeField] [Tooltip("Drag a GameObject there. This object will be considered as Destination")] private GameObject TeleportDestination;

    [SerializeField] [Tooltip("What exactly to teleport (Usually it's a Player)")] private GameObject ThingToTeleportate;

    [HideInInspector]
    public Vector3 positionOfDestination;

    /// <summary>
    /// Use this position, to 
    /// </summary>
    public void Teleport()
    {
        positionOfDestination = TeleportDestination.transform.position;
        ThingToTeleportate.transform.position = positionOfDestination;

    }
}

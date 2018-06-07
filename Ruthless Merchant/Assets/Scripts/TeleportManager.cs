using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeleportEnum : MonoBehaviour {

    //Destinations for Unity Inspector when 
    public enum Destination
    {
        PointA, PointB, PointC,PointD, PointE
    }

    [SerializeField]
    TeleportEnum myEnum;


    public Destination TeleportDestination;

    public void Teleport()
    {
        switch (TeleportDestination)
        {
            case Destination.PointA:
                Debug.Log(TeleportDestination);
                break;
            case Destination.PointB:
                Debug.Log(TeleportDestination);
                break;
            case Destination.PointC:
                Debug.Log(TeleportDestination);
                break;
            case Destination.PointD:
                Debug.Log(TeleportDestination);
                break;
            case Destination.PointE:
                Debug.Log(TeleportDestination);
                break;
                
        }
    }
}

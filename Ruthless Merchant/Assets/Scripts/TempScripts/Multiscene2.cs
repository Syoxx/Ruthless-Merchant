using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiscene2 : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Loggin'");
        Multiscene.Singleton.Log();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiscene : MonoBehaviour
{
    public static Multiscene Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void Log()
    {
        Debug.Log("AAAAA");
    }
}

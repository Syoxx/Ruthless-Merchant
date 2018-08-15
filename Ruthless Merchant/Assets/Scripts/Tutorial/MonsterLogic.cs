using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterLogic : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition;

    [SerializeField] private GameObject destination;

    [SerializeField] private int speed;

    void Start()
    {
        startPosition = transform.position;
        endPosition = destination.transform.position;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);
    }

    
}

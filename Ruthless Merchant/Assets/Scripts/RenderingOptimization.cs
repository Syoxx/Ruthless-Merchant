using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingOptimization : MonoBehaviour
{

    private List<GameObject> listOfRenderedObjects;

    [SerializeField] [Tooltip("The distance when the Object dissapears")]
    private int distance;

    private float elapsedTime = 0;
    [SerializeField, Tooltip("Limits the updates to increase performance")]
    private float updateinterval = 0.064f;
	// Use this for initialization
	void Start ()
    {
        elapsedTime = updateinterval;
	    listOfRenderedObjects = new List<GameObject>();

        //Find all objects with a tag and add them to list
	    foreach (GameObject renderinGameObject in GameObject.FindGameObjectsWithTag("Rendering"))
	    {
	        listOfRenderedObjects.Add(renderinGameObject);
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckForRendering();
    }

    /// <summary>
    /// Checks the distance between a GameObject and Player. If the distance is huge -> deactivate rendering object
    /// </summary>
    private void CheckForRendering()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > updateinterval)
        {
            elapsedTime = 0.0f;
            foreach (var renderedObject in listOfRenderedObjects)
            {
                float tempDistance = Vector3.Distance(transform.position, renderedObject.transform.position);
                if (tempDistance < distance)
                {
                    renderedObject.SetActive(true);
                }

                if (tempDistance > distance)
                {
                    renderedObject.SetActive(false);
                }
            }
        }
    }
}

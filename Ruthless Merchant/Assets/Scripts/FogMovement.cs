using UnityEngine;

public class FogMovement : MonoBehaviour
{
    public GameObject FogObject;

	// Update is called once per frame
	void Update ()
    {
        if(FogObject != null)
            FogObject.transform.position = transform.position;
	}
}

using UnityEngine;

public class FogMovement : MonoBehaviour
{
    public GameObject FogObject;

	// Update is called once per frame
	void Update ()
    {
        if (FogObject != null)
        {
            FogObject.transform.position = new Vector3(transform.position.x, FogObject.transform.position.y, transform.position.z);
        }
	}
}

using UnityEngine;

public class FogMovement : MonoBehaviour
{
    public GameObject FogObject;

    [SerializeField]
    private float maxHeight = 25;

	// Update is called once per frame
	void Update ()
    {
        if (FogObject != null)
        {

            FogObject.transform.position = new Vector3(transform.position.x, transform.position.y > maxHeight ? FogObject.transform.position.y : transform.position.y, transform.position.z);
        }
	}
}

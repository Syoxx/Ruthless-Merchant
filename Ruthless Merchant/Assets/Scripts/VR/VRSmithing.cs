using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VRSmithing : MonoBehaviour
{
    public static VRSmithing Singleton;

    [SerializeField]
    Transform[] ironFinalPositions;

    public GameObject[] Irons;

    [SerializeField]
    public GameObject FinalSword;

    public bool AllIronsPlaced;

    private void Awake()
    {
        Singleton = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject iron in Irons)
        {
            if (collision.gameObject == iron)
            {
                Vector3 finalPosition = Vector3.zero;
                Quaternion finalRotation = new Quaternion();

                for (int x = 0; x < ironFinalPositions.Length; x++)
                {
                    if (ironFinalPositions[x].position != Vector3.zero)
                    {
                        finalPosition = ironFinalPositions[x].position;
                        finalRotation = ironFinalPositions[x].rotation;
                        ironFinalPositions[x].position = Vector3.zero;

                        if (x == ironFinalPositions.Length - 1)
                            AllIronsPlaced = true;

                        break;
                    }
                }

                iron.transform.position = finalPosition;
                iron.transform.rotation = finalRotation;

                Destroy(iron.GetComponent<Throwable>());
                Destroy(iron.GetComponent<VelocityEstimator>());
                Destroy(iron.GetComponent<Interactable>());
                iron.GetComponent<Rigidbody>().isKinematic = true;

                break;
            }
        }
    }
}

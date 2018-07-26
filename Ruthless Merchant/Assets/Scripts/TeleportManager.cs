using RuthlessMerchant;
using UnityEngine;

public class TeleportManager : MonoBehaviour {

    //By Daniil Masliy

    #region Serialize Fields
    [SerializeField] [Tooltip("Drag a GameObject there. This object will be considered as Destination")]
    private GameObject teleportDestination;
    #endregion

    #region Private Fields
    private Rigidbody rb;
    private Player playerScript;
    private Vector3 positionOfDestination;
    #endregion


    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        playerScript = GetComponentInParent<Player>();
    }

    /// <summary>
    /// Use this position, to declare where to teleport
    /// </summary>
    public void Teleport()
    {
        playerScript.Grounding(false);
        positionOfDestination = teleportDestination.transform.position;
        rb.transform.position = positionOfDestination;

    }
}

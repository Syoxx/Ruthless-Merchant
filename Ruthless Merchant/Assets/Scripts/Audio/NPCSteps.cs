using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSteps : MonoBehaviour {

    [SerializeField, Tooltip("Speed of the stepsounds")]
    public float stepSpeed = 0.4f;

    [FMODUnity.EventRef]
    public string Steps;

    private NavMeshAgent agent;
    private bool characterIsMoving;


	// Use this for initialization
	void Start () {
        agent = this.GetComponent<NavMeshAgent>();
        InvokeRepeating("CallFootsteps", 0, stepSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.velocity.x > 0.05f || agent.velocity.y > 0.05f || agent.velocity.z > 0.05f)
        {
            characterIsMoving = true;
        }
        else if (agent.velocity.x <= 0.05f || agent.velocity.y <= 0.05f)
        {
            characterIsMoving = false;
        }
	}

    void CallFootsteps()
    {
        if (characterIsMoving == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(Steps, this.GetComponent<Transform>().position);
        }
    }
}

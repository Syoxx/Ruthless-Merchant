using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField]
    protected string Description; //{ get; set; }
    //[SerializeField]
    protected int RequiredAmount; //{ get; set; }
    [SerializeField]
    protected List<Transform> Waypoints;

    public bool Completed; //{ get; set; }
    protected int CurrentAmount; //{ get; set; }


    public virtual void Initialize()
    {

    }
    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        Completed = true;
    }
}

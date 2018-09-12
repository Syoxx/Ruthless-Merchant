using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    /// <summary>
    ///base class for quests taht every quest type inherits from
    /// </summary>

    [SerializeField]
    public string QuestTitle;
    [SerializeField]
    public string Description; 
    //[SerializeField]
    public int RequiredAmount; 
    [SerializeField]
    public int Reward;              
    [SerializeField]
    public float ReputationGain;    

    public bool Completed; 
    public bool InProgress;
    public int CurrentAmount; 


    public virtual void Initialize()
    {

    }

    /// <summary>
    ///Evaluates if the quest is completed
    /// </summary>
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

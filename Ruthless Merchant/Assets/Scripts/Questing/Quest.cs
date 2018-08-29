using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour {
    [SerializeField]
    private int QuestID;
    [SerializeField]
    private string QuestName;
    [SerializeField]
    private string Description;
    [SerializeField]
    private int GoldReward;

    private List<Goal> Goals;
    private bool Completed; 



    public void Start()
    {   
        Goals = GetComponents<Goal>().ToList();        
    }
    
    public void Update()
    {
        CheckGoals();
    }
    public void CheckGoals()
    {
         Completed = Goals.All(g => g.Completed);
        if (Completed)
            GiveReward();
    }

    void GiveReward()
    {

    }
}

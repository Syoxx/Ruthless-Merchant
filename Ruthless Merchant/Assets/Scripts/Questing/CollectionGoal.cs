using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal {

    [SerializeField]
    private int CollectableID;

    public CollectionGoal(int collectableID, string description, bool completed, int currentAmount, int requiredAmount, List<Transform>waypoints)
    {
        CollectableID = collectableID;
        Description = description;
        Completed = completed;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
        Waypoints = waypoints;
    }

    public override void Initialize()
    {
        base.Initialize();

    }

    void CollectableFound()
    {
        CurrentAmount++;
        Evaluate();
    }
}

// Author: Richard Brönnimann

using RuthlessMerchant;
using UnityEngine;

public class Reputation : MonoBehaviour {

    #region fields
    [SerializeField, Tooltip("Player standing with the Freidenker faction")]
    [Range(0, 1)]
    private float freidenkerStanding;

    [SerializeField, Tooltip("Player standing with the Imperialisten faction")]
    [Range(0, 1)]
    private float imperalistenStanding;

    [SerializeField, Tooltip("Maximum Reputation")]
    private float maxReputation;

    private bool freidenkerMaxAchieved, imperialistenMaxAchieved;
    #endregion

    #region properties
    public float ImperalistenStanding
    {
        get
        {
            return imperalistenStanding;
        }

        set
        {
            imperalistenStanding = value;
        }
    }

    public float FreidenkerStanding
    {
        get
        {
            return freidenkerStanding;
        }

        set
        {
            freidenkerStanding = value;
        }
    }

    public float MaxReputation
    {
        get
        {
            return maxReputation;
        }
    }

    public bool FreidenkerMaxAchieved
    {
        get
        {
            return freidenkerMaxAchieved;
        }
    }

    public bool ImperialistenMaxAchieved
    {
        get
        {
            return imperialistenMaxAchieved;
        }
    }
    #endregion

    #region methods
    void Start ()
    {
        FreidenkerStanding = 0f;
        ImperalistenStanding = 0f;
        freidenkerMaxAchieved = false;
        imperialistenMaxAchieved = false;
    }

    private void Update()
    {
        if (freidenkerStanding >= maxReputation)
            freidenkerMaxAchieved = true;
        if (imperalistenStanding >= maxReputation)
            imperialistenMaxAchieved = true;
    }


    /// <summary>
    /// This method allows faction standings to be modified
    /// </summary>
    /// <param name="affectedFaction">
    /// Which faction standing is changed
    /// </param>
    /// <param name="standingDifference">
    /// Standing is modified by this value (positive or negative)
    /// </param>
    public void ChangeStanding(Faction affectedFaction, float standingDifference)
    {
        switch (affectedFaction)
        {
            case Faction.Freidenker:
                {
                    freidenkerStanding += standingDifference;
                    freidenkerStanding = (freidenkerStanding > 1f) ? 1f : freidenkerStanding;
                }
                break;
            case Faction.Imperialisten:
                {
                    imperalistenStanding += standingDifference;
                    imperalistenStanding = (imperalistenStanding > 1f) ? 1f : imperalistenStanding;
                }
                break;
            default:
                Debug.Log("Attempted to change a faction standing that doesn't exist.");
                break;
        }
    }
    #endregion

}

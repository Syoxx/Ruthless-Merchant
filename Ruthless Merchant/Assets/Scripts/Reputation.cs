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
    #endregion

    #region methods
    void Start ()
    {
        FreidenkerStanding = 0f;
        ImperalistenStanding = 0f;
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

// Author: Richard Brönnimann

using RuthlessMerchant;
using UnityEngine;

public class TradepointUnlocker : InteractiveObject
{
    #region fields

    [SerializeField, Tooltip("Object that visualizes that this outpost was upgraded.")]
    private GameObject TradingPointTent;

    [SerializeField, Tooltip("Identifies the local outpost (Imperial base is 1, Freidenker base is 9)")]
    private int OutpostIndex;
    #endregion

    #region methods

    public override void Interact(GameObject caller)
    {
        Player player = caller.GetComponent<Player>();
        player.OutpostInteraction(OutpostIndex);
    }

    public void SetActiveTradepoint()
    {
        // make tradingpoint visible
        Debug.Log("trading point purchased");
        //gameObject.GetComponentInChildren<Renderer>().enabled = false;    // this makes the unlocker invisible, can still receive player interaction tho
        TradingPointTent.SetActive(true);
        Achievements.AddToCounter(null, false);
    }

    public override void Start() { }

    public override void Update() { }
    #endregion
}

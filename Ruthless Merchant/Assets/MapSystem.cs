// Author: Richard Brönnimann

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour {
    #region fields

    private GameObject MapObject;    
    private List<Button> MapButtons;
    #endregion

    #region methods

    public void Start () {

        //Get map object proper
        MapObject = gameObject;

        // Get each individual button
        if (MapButtons == null)
        {
            MapButtons = new List<Button>();

            int numOfButtons = transform.childCount;

            for (int i = 2; i < numOfButtons; i++)
            {
                Transform MapElement = transform.GetChild(i);
                if (MapElement.GetChild(1).GetComponent<Button>() != null)
                {
                    MapButtons.Add(MapElement.GetChild(1).GetComponent<Button>());
                    //Debug.Log(mapElement.name);
                }
                else if (MapElement.GetChild(0).GetComponent<Button>() != null) // incase buttons are not at the expected index in MapElement
                {
                    MapButtons.Add(MapElement.GetChild(0).GetComponent<Button>());
                }
            }

            for (int buttonIndex = 0; buttonIndex < MapButtons.Count; buttonIndex++)
            {
                MapButtons[buttonIndex].interactable = false;
            }
        }
	}
	
	// Updates map elements
	public void RefreshMapCanvas(bool[] unlockedTravelPoints)
    {
        // iterate through array, activate unlocked fast travel points
        //foreach (bool travelPoint in unlockedTravelPoints)
        for (int i = 0; i < unlockedTravelPoints.Length; i++)
        {
            if (unlockedTravelPoints[i] == true)
            {
                MapButtons[i - 1].interactable = true;
            }
        }
	}
    #endregion
}

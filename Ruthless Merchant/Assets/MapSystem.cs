// Author: Richard Brönnimann

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour {
    #region fields
    private GameObject MapObject;

    [SerializeField]
    private List<Button> MapButtons = new List<Button>();
    #endregion

    #region methods
    void Start () {

        //Get map object proper
        MapObject = gameObject;

        // Get each individual button
        //foreach (Button mapPoint in MapObject)
        int numOfButtons = transform.childCount;

        for (int i = 1; i < numOfButtons; i++)
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
	
	// Updates map elements
	public void RefreshMapCanvas ()
    {
		// iterate through array
        // if != null, activate button at the index given 
	}
    #endregion
}

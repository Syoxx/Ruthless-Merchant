using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class VR_UIInput : MonoBehaviour
{
    [SerializeField]
    private Light continueLight, exitLight, startLight, creditsLight;

    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    private void Start()
    {
        continueLight.enabled = false;
        exitLight.enabled = false;
        startLight.enabled = false;
        creditsLight.enabled = false;
    }

    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn -= HandlePointerIn;
        laserPointer.PointerIn += HandlePointerIn;
        laserPointer.PointerOut -= HandlePointerOut;
        laserPointer.PointerOut += HandlePointerOut;

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        if(e.target.tag == "MenuObject")
        {
            string goName = e.target.name;

            switch(goName)
            {
                case "Continue":
                    Continue(true);
                    break;
                case "StartGame":
                    StartGame(true);
                    break;
                case "Credits":
                    Credits(true);
                    break;
                case "QuitGame":
                    QuitGame(true);
                    break;
            }
        }
        var button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.Select();
            Debug.Log("HandlePointerIn", e.target.gameObject);
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "MenuObject")
        {
            string goName = e.target.name;

            switch (goName)
            {
                case "Continue":
                    Continue(false);
                    break;
                case "StartGame":
                    StartGame(false);
                    break;
                case "Credits":
                    Credits(false);
                    break;
                case "QuitGame":
                    QuitGame(false);
                    break;
            }
        }
        var button = e.target.GetComponent<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("HandlePointerOut", e.target.gameObject);
        }
    }

    private void QuitGame(bool activated)
    {
        if (activated)
        {
            exitLight.enabled = true;
        }

        else
        {
            exitLight.enabled = false;
        }
    }

    private void Credits(bool activated)
    {
        if (activated)
        {
            creditsLight.enabled = true;
        }

        else
        {
            creditsLight.enabled = false;
        }
    }

    private void StartGame(bool activated)
    {
        if (activated)
        {
            startLight.enabled = true;
        }

        else
        {
            startLight.enabled = false;
        }
    }

    private void Continue(bool activated)
    {
        if (activated)
        {
            startLight.enabled = true;
        }

        else
        {
            startLight.enabled = false;
        }
    }
}
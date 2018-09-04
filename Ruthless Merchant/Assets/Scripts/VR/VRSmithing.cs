using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace RuthlessMerchant
{
    public class VRSmithing : MonoBehaviour
    {
        public static VRSmithing Singleton;

        public enum SmithingSteps
        {
            PlacingSingleIrons,
            MeltingIron,
            PlacingMeltedIron,
            CreatingSword,
            Done
        }

        public SmithingSteps smithingSteps;

        public Transform[] IronFinalPositions;

        public GameObject[] Irons;

        public GameObject FinalSword;

        public GameObject HammerItem;

        public GameObject HammerController1;
        public GameObject HammerController2;

        public Hand Hand1;
        public Hand Hand2;

        public GameObject[] ControllerObjectsToDeactivate1;
        public GameObject[] ControllerObjectsToDeactivate2;

        [System.NonSerialized]
        public Hand HammerHand;

        public GameObject Meltbox;
        public GameObject MeltedIron;

        public GameObject HotIron;

        public GameObject EmptyMeltbox;

        private void Awake()
        {
            Singleton = this;
            smithingSteps = SmithingSteps.PlacingSingleIrons;
        }

        private void Update()
        {
            switch(smithingSteps)
            {
                case SmithingSteps.CreatingSword:
                    CreatingSwordStep();
                    break;
            }
        }

        public void PlacingSingleIronsStep(Collision collision)
        {
            foreach (GameObject iron in Irons)
            {
                if (collision.gameObject == iron)
                {
                    Vector3 finalPosition = Vector3.zero;
                    Quaternion finalRotation = new Quaternion();

                    for (int x = 0; x < IronFinalPositions.Length; x++)
                    {
                        if (IronFinalPositions[x].position != Vector3.zero)
                        {
                            finalPosition = IronFinalPositions[x].position;
                            finalRotation = IronFinalPositions[x].rotation;
                            IronFinalPositions[x].position = Vector3.zero;

                            if (x == IronFinalPositions.Length - 1)
                            {
                                smithingSteps = SmithingSteps.MeltingIron;
                                Invoke("MakeMeltBoxInteractbale", 1);
                            }

                            break;
                        }
                    }

                    Destroy(iron.GetComponent<Throwable>());
                    Destroy(iron.GetComponent<VelocityEstimator>());
                    Destroy(iron.GetComponent<Interactable>());
                    Destroy(iron.GetComponent<BoxCollider>());
                    Destroy(iron.GetComponent<Rigidbody>());

                    iron.transform.position = finalPosition;
                    iron.transform.rotation = finalRotation;
                    iron.transform.parent = Meltbox.transform;

                    break;
                }
            }
        }

        void MakeMeltBoxInteractbale()
        {
            Meltbox.AddComponent<Rigidbody>();
            Meltbox.AddComponent<Interactable>();
            Meltbox.AddComponent<Throwable>();
            Meltbox.AddComponent<Outline>();

            for(int x = Irons.Length - 1; x >= 0; x--)
            {
                Destroy(Irons[x]);
            }

            MeltedIron.SetActive(true);
            smithingSteps = SmithingSteps.PlacingMeltedIron;
        }

        void CreatingSwordStep()
        {
            if (Hand1 != null && Hand1.controller.GetHairTriggerDown())
            {
                foreach (VRSceneItem item in FindObjectsOfType<VRSceneItem>())
                {
                    HammerHand = Hand1;
                    StartCoroutine(item.ChangeToHammerController(1));
                }
            }

            else if (Hand2 != null && Hand2.controller.GetHairTriggerDown())
            {
                foreach (VRSceneItem item in FindObjectsOfType<VRSceneItem>())
                {
                    HammerHand = Hand2;
                    StartCoroutine(item.ChangeToHammerController(2));
                }
            }
        }

        public void PlacingMeltedIronStep(Collision collision)
        {
            if (collision.gameObject == Meltbox)
            {
                HotIron.SetActive(true);
                EmptyMeltbox.SetActive(true);
                Destroy(Meltbox);

                smithingSteps = SmithingSteps.CreatingSword;
            }
        }

        public void CreatingSwordStep(Collision collision)
        {
            if (collision.gameObject == HammerController1 || collision.gameObject == HammerController2)
            {
                Destroy(HotIron);
                FinalSword.SetActive(true);
                HammerHand.controller.TriggerHapticPulse(UInt16.MaxValue);

                smithingSteps = SmithingSteps.Done;
            }
        }
    }
}
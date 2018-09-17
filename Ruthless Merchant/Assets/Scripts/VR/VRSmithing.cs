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
            PlacingSword,
            Trading
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
        public GameObject MeltedMeltbox;

        [SerializeField]
        GameObject fireEffectsParent;

        public GameObject HotIron;

        public GameObject EmptyMeltbox;

        [SerializeField]
        GameObject swordPlaceholder;

        [SerializeField]
        GameObject meltedIronPlaceholder;

        [SerializeField]
        GameObject hammerPlaceholder;

        [SerializeField]
        GameObject meltedIronLiquid;

        public GameObject SellingContract;

        public GameObject rejectedContract;

        float timer = 0;

        Transform[] allIrons;

        public GameObject FireworkEnd;

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

                case SmithingSteps.MeltingIron:
                    MeltingIronStep();
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
                                fireEffectsParent.SetActive(true);
                                Invoke("SetStepToMelting", 1);
                                Invoke("MakeMeltBoxInteractbale", 6);
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

        void SetStepToMelting()
        {
            smithingSteps = SmithingSteps.MeltingIron;
        }

        void MeltingIronStep()
        {
            timer += Time.deltaTime / 4;
            meltedIronLiquid.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, -0.0979f), new Vector3(0, 0, 0.0085f), timer);

            if(allIrons == null)
                allIrons = Meltbox.GetComponentsInChildren<Transform>();

            foreach (Transform iron in allIrons)
            {
                if (iron.gameObject == Meltbox || iron.gameObject == meltedIronLiquid)
                    continue;

                if (iron.localScale.x <= 0)
                    break;

                float scaleDelta = Time.deltaTime / 4;

                iron.localScale -= new Vector3(scaleDelta, scaleDelta, scaleDelta);
            }
        }

        void MakeMeltBoxInteractbale()
        {
            for(int x = Irons.Length - 1; x >= 0; x--)
            {
                Destroy(Irons[x]);
            }

            MeltedMeltbox.transform.position = Meltbox.transform.position;
            MeltedMeltbox.transform.rotation = Meltbox.transform.rotation;

            Destroy(Meltbox);
            MeltedMeltbox.SetActive(true);
            meltedIronPlaceholder.SetActive(true);

            smithingSteps = SmithingSteps.PlacingMeltedIron;
        }

        void CreatingSwordStep()
        {
            if (Hand1.controller != null && Hand1.controller.GetHairTriggerDown())
            {
                foreach (VRSceneItem item in FindObjectsOfType<VRSceneItem>())
                {
                    HammerHand = Hand1;
                    StartCoroutine(item.ChangeToHammerController(1));
                }
            }

            else if (Hand2.controller != null && Hand2.controller.GetHairTriggerDown())
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
            if (collision.gameObject == MeltedMeltbox)
            {
                HotIron.SetActive(true);
                EmptyMeltbox.SetActive(true);
                Destroy(MeltedMeltbox);
                meltedIronPlaceholder.SetActive(false);
                hammerPlaceholder.SetActive(true);

                smithingSteps = SmithingSteps.CreatingSword;
            }
        }

        public void CreatingSwordStep(Collision collision)
        {
            if (collision.gameObject.name.ToLower().Contains("hammer"))
            {
                Destroy(HotIron);
                FinalSword.SetActive(true);
                hammerPlaceholder.SetActive(false);

                if (HammerHand == Hand1)
                {
                    HammerController1.SetActive(false);
                    HammerItem.transform.position = HammerController1.transform.position;
                    HammerItem.transform.rotation = HammerController1.transform.rotation;
                }

                else if (HammerHand == Hand2)
                {
                    HammerController2.SetActive(false);
                    HammerItem.transform.position = HammerController2.transform.position;
                    HammerItem.transform.rotation = HammerController2.transform.rotation;
                }

                HammerItem.SetActive(true);
                smithingSteps = SmithingSteps.PlacingSword;
                swordPlaceholder.SetActive(true);

                if (HammerHand != null)
                    HammerHand.controller.TriggerHapticPulse(UInt16.MaxValue);
            }
        }

        public void PlacingSwordStep(Collision collision)
        {
            if(smithingSteps == SmithingSteps.PlacingSword && collision.gameObject.name.ToLower().Contains("sword"))
            {
                swordPlaceholder.SetActive(false);

                FinalSword.transform.position = swordPlaceholder.transform.position;
                FinalSword.transform.rotation = swordPlaceholder.transform.rotation;

                Destroy(FinalSword.GetComponent<Throwable>());
                Destroy(FinalSword.GetComponent<Interactable>());
                Destroy(FinalSword.GetComponent<VelocityEstimator>());
                Destroy(FinalSword.GetComponent<Rigidbody>());
                Destroy(FinalSword.GetComponent<Collider>());

                smithingSteps = SmithingSteps.Trading;
                SellingContract.SetActive(true);
                TradeAbstract.Singleton.Initialize();
            }
        }
    }
}
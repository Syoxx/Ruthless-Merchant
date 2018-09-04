//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial Singleton;
        public bool isTutorial;

        private bool monsterTriggered = false;

        [SerializeField]
        private int minCollectedVains = 1;
        [SerializeField]
        private int minCollectedPlants = 1;

        [SerializeField, Tooltip("Add all collactable materials")]
        private GameObject[] ironVains;

        [SerializeField, Tooltip("Add platns here")]
        private GameObject[] plants;

        //Check bool to start the movement of Monster
        [System.NonSerialized]
        public bool TradeIsDone;

        [SerializeField]
        private Trader trader;

        [SerializeField, Tooltip("Drag Monster from tutorial there")]
        private Monster monster;

        [SerializeField]
        [Tooltip("Drag DefaultSword Prefab there")]
        private Item defaultSword;
        [SerializeField]
        [Tooltip("Drag IronSword Prefab there")]
        private Item ironSword;
        [SerializeField]
        private GameObject playerCaveDoor;

        [SerializeField]
        [Tooltip("Drag Player there")]
        private GameObject PlayerObject;
        [SerializeField]
        [Tooltip("Drag ExitOutOfCave from Tutorial there")]
        private GameObject exitOutOfCaveObject;
        [SerializeField]
        [Tooltip("Drag SmithTeleport from Tutorial there")]
        private GameObject smithEnterObject;
        [SerializeField]
        [Tooltip("Drag SmithExit from Tutorial there")]
        private GameObject traderTigger;
        [SerializeField]
        [Tooltip("Drag Textmesh from Tutorial/Subtitle there")]
        private TextMeshProUGUI textMesh;

        private Collider playerCollider, exitOutOfCaveCollider;
        [SerializeField]
        [Tooltip("Drag FadeImage there")]
        private Image myFade;

        //Dialogue Trigger Zones
        [SerializeField]
        [Tooltip("Drag 10_TriggerZone from Tutorial there")]
        private GameObject dialogueZone10;
        [SerializeField]
        [Tooltip("Drag Tutorial there")]
        private GameObject TutorialObject;

        [SerializeField]
        [Tooltip("Drag AlchemyTextTrigger from Tutorial there")]
        private GameObject AlchemyTextTrigger;
        [SerializeField]
        [Tooltip("Drag WorkbenchTrigger from Tutorial there")]
        private GameObject WorkbenchTextTrigger;

        [SerializeField]
        private GameObject tutorialCave;

        [SerializeField]
        private Material itemSteel;

        [SerializeField]
        private GameObject traderPart;

        [SerializeField]
        private GameObject monsterTarget;

        [SerializeField]
        private Guard guard1;

        [SerializeField]
        private Guard guard2;

        //Dialogue colliders
        private Collider dialogueZone11Collider;

        private bool SmithDone, WorkbenchDone, AlchemyDone, CollectingDone, SkipCollection;

        [SerializeField]
        private string startMonolog = " Some call it a shattered world … and I call it home. Not because of my origin. But because I strive to own this place. I can move around with [W][A][S][D], jump with [SPACE], run with [SHIFT], look around by moving the mouse and collect items or interact by pressing [E]. Now I should collect some materials.";
        [SerializeField]
        private string tryExitWithoutMaterials = "I think I should collect some materials before I leave";
        [SerializeField]
        private string collectedMaterials = "That should be enough material for now. I should find an exit out of this cave.";
        [SerializeField]
        private string playerCaveEntered = "Now that I have arrived at my cave, I should get to work.";
        [SerializeField]
        private string playerCaveCraftAlchemy = "At alchemy stations like this one, I can brew potions to sell. I just put ingredients in the small bottles and combine their effects with the tools on the table. I can look at the three different bottles and insert something by pressing [E]. After I added all the ingredients I want I just press [E] while looking at the station and my potion should be brewed and be in my inventory.";
        [SerializeField]
        private string playerCaveCraftSmith = "There he is, one of the best smiths around. I only have to bring him materials and he will forge whatever weapon I need. By pressing the [E] key while facing him I can see all recipes. Then I choose the right one by clicking the left mouse button.";
        [SerializeField]
        private string playerCaveCraftDismantel = "Sometimes, I need to dismantle some of my old wares. This workbench is the perfect tool to do so, and still retrieve some materials. I should look at the workbench and press [E]. Then I just select the old item by clicking the left mouse button.";
        [SerializeField]
        private string playerCaveFinished = "'Everything is set and prepared. I should leave this place and make some profit by selling my carefully crafted goods. I can start with this trader in front of me. I just need to look at him and press [E] to start the trade.'";
        [SerializeField] //Nachdem man E auf den Händler gedrückt hat
        private string TradingInit = "On the bottom right I can see a list of all items I want to seel now. I can add and remove items by just clicking the left mouse button while pointing at them in the inventory or the selling list. As soon as Im happy with my selection I hit start trade.";
        [SerializeField] //Bei Start des Handels
        private string TradingDoBet = "On the right side I can see my offered price. By turning the mouse wheel I can increase or decrease it. Then I click the left mouse button to see the trader´s offer. As soon as Im happy with the price I press [E] to accept the trade at the price of the trader.";
        [SerializeField]
        private string TradeGoodBet = "'I preferred the chance for profit. He had a sense for that and replied with a lower initial offer. Now, we both compromised until we came to an agreement – or someone dropped the whole deal'";
        [SerializeField]
        private string TradeBadBet = "'I opened in a moderate fashion. I could go only lower from there'";
        [SerializeField]
        private string TradeGoodResult = "'I made a profitable deal by selling it for a good price.'";
        [SerializeField]
        private string TradeBadResult = "'I may have missed on that price, but a sold item is still a sold item I don’t have to carry around. And my customer seemed happy. I should quickly move to the nearby cave.'";
        [SerializeField]
        private string TradeIrritationResultGreedy = "'I was too greedy. I expected too much from this deal. Honestly, I would have declined an offer like that too. I should quickly move to the nearby cave.'";
        [SerializeField]
        private string TradeIrritationResultNaiv = "'I was still naive and dropped the price way to easy. No wonder he couldn’t take me serious anymore. I should quickly move to the nearby cave.'";
        [SerializeField]
        private string TradeFinished = "Holy Eviternity, a monster is approaching. I hope the guards take care of him.";

        private void Awake()
        {
            Singleton = this;
        }

        void Start()
        {
            textMesh = textMesh.GetComponent<TextMeshProUGUI>();
            playerCollider = PlayerObject.GetComponent<Collider>();
            exitOutOfCaveCollider = exitOutOfCaveObject.GetComponent<Collider>();

            // Dialogues
            dialogueZone11Collider = dialogueZone10.GetComponent<Collider>();


            Player.Singleton.Inventory.Add(defaultSword, 1, true);

            myFade.FadingWithCallback(1, 0.001f, delegate
            {
                Monolog(startMonolog); myFade.FadingWithCallback(0, 3, delegate { Debug.Log("Done fading"); });

                //Sound - triger voiceline 1
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 1", Player.Singleton.transform.position);
            });

            monster.SetCurrentAction(new ActionIdle(ActionNPC.ActionPriority.High), null, true, true);
        }

        // Update is called once per frame
        void Update()
        {
            CheckCollection();
            OpenSmithDoor();

            if(TradeIsDone)
            {
                if (!monsterTriggered)
                {
                    monster.SetCurrentAction(new ActionMove(ActionNPC.ActionPriority.Medium), trader.transform.gameObject, true, true);
                    guard1.SetCurrentAction(new ActionIdle(ActionNPC.ActionPriority.Low), null, true, true);
                    guard2.SetCurrentAction(new ActionIdle(ActionNPC.ActionPriority.Low), null, true, true);
                    Monolog(TradeFinished);
                    monsterTriggered = true;

                    //Sound - trigger voiceline 15
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 15", Player.Singleton.transform.position);
                }
                else
                {
                    if (monster == null || monster.HealthSystem.Health <= 0)
                    {
                        TutorialFinished();
                    }
                }
            }
        }

        /// <summary>
        /// Tutorial finished disable all tutorial elements
        /// </summary>
        private void TutorialFinished()
        {
            //TODO Fade to black
            myFade.FadingWithCallback(1, 3.5f, delegate
            {
                DisableTutorial();
                myFade.FadingWithCallback(0, 3, delegate { Debug.Log("Tutorial disabled"); });
            });
        }

        /// <summary>
        /// Check how many items were collected by the player in the first cave
        /// </summary>
        private void CheckCollection()
        {
            if (tutorialCave.activeSelf)
            {
                int collectedVains = 0;
                if (ironVains != null)
                {
                    for (int i = 0; i < ironVains.Length; i++)
                    {
                        if (ironVains[i] == null)
                            collectedVains++;
                    }
                }

                int collectedPlants = 0;
                if (plants != null)
                {
                    for (int i = 0; i < plants.Length; i++)
                    {
                        if (plants[i] == null)
                            collectedPlants++;
                    }
                }

                int steelCollected = Player.Singleton.Inventory.GetNumberOfItems(itemSteel);
                bool prevDone = CollectingDone;
                CollectingDone = collectedPlants >= minCollectedPlants && collectedVains >= minCollectedVains && steelCollected > 3;

                if (!prevDone && (CollectingDone || SkipCollection))
                {
                    Monolog(Singleton.collectedMaterials);

                    //Sound - trigger voiceline 
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line ", Player.Singleton.transform.position);
                }
            }
        }

        #region TradeMonologs
        /// <summary>
        /// Starts trading and show the corresponding monolog (Adds a sword when the player doesn't have one)
        /// </summary>
        public void StartTrading()
        {
            traderTigger.SetActive(false);
            int swords = Player.Singleton.Inventory.GetNumberOfItems(ironSword);
            if (swords <= 0)
                Player.Singleton.Inventory.Add(ironSword, 1, true);


            Monolog(TradingInit);
            trader.Interact(Player.Singleton.gameObject);
        }

        /// <summary>
        /// Start monolog when the player places the first bet
        /// </summary>
        public void TraderItemSelectionMonolog2()
        {
            Monolog(TradingDoBet);
        }

        /// <summary>
        /// Start monolog when the player places the first bet
        /// </summary>
        public void TraderMonolog3()
        {
            Monolog(TradingDoBet);
        }

        /// <summary>
        /// Good monolog response when the player did a good bet
        /// </summary>
        public void TraderMonolog4()
        {
            Monolog(TradeGoodBet);

            //Sound - trigger voiceline 9
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 9", Player.Singleton.transform.position);
        }

        /// <summary>
        /// Bad monolog response when the player did a bad bet
        /// </summary>
        public void TradeMonolog5()
        {
            Monolog(TradeBadBet);

            //Sound - trigger voiceline 10
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 10", Player.Singleton.transform.position);
        }

        /// <summary>
        /// Good monolog response when the player did a good trade
        /// </summary>
        public void TradeMonolog6()
        {
            Monolog(TradeGoodResult);

            //Sound - trigger voiceline 11
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 11", Player.Singleton.transform.position);
        }

        /// <summary>
        /// Bad monolog response when the player did a bad trade
        /// </summary>
        public void TraderMonolog7()
        {
            Monolog(TradeBadResult);

            //Sound - trigger voiceline 12
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 12", Player.Singleton.transform.position);
        }

        /// <summary>
        /// Greedy irritation response
        /// </summary>
        public void TraderMonolog8()
        {
            Monolog(TradeIrritationResultGreedy);

            //Sound - trigger voiceline 13
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 13", Player.Singleton.transform.position);
        }

        /// <summary>
        /// Naiv irritation response
        /// </summary>
        public void TraderMonolog9()
        {
            Monolog(TradeIrritationResultNaiv);

            //Sound - trigger voiceline 14
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 14", Player.Singleton.transform.position);
        }
        #endregion

        /// <summary>
        /// Sets and shows the monolog text
        /// </summary>
        /// <param name="text">Text of monolog</param>
        public void Monolog(string text)
        {
            if (isTutorial && textMesh != null)
            {
                textMesh.text = text;
                textMesh.transform.parent.gameObject.SetActive(true);
                Debug.Log("ChangeText: " + text);
            }
            else
            {
                Debug.Log("ChangeTextFailed: " + text);
            }
        }

        /// <summary>
        /// Opens the player cave door when the player used the workbench, alchemy and the smith
        /// </summary>
        public void OpenSmithDoor()
        {
            if (WorkbenchDone && AlchemyDone && SmithDone && playerCaveDoor.activeSelf)
            {
                Monolog(playerCaveFinished);

                //Sound - trigger voiceline 8
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 8", Player.Singleton.transform.position);

                playerCaveDoor.SetActive(false);
            }
        }

        /// <summary>
        /// Disables all tutorial objects
        /// </summary>
        public void DisableTutorial()
        {
            TutorialObject.SetActive(false);
            tutorialCave.SetActive(false);
            traderPart.SetActive(false);
        }

        /// <summary>
        /// Sets SmithDone which can be used  to skip the smith/crafting part of the tutorial
        /// </summary>
        public void SmithIsCompleted()
        {
            SmithDone = true;
        }

        /// <summary>
        /// Set AlchemyDone to true which can be used to skip the alchemy part of the tutorial
        /// </summary>
        public void AlchemyIsCompleted()
        {
            AlchemyDone = true;
        }

        /// <summary>
        /// Sets WorkbenchDone to true which can be used to skip the workbench part of the tutorial
        /// </summary>
        public void WorkbenchIsCompleted()
        {
            WorkbenchDone = true;
        }

        /// <summary>
        /// Sets SkipCollection to true
        /// </summary>
        public void CollectionIsCompleted()
        {
            SkipCollection = true;
        }

        /// <summary>
        /// CHeck trigger activations
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.name)
            {
                case "TradeTrigger":
                    StartTrading();
                    break;
                case "AlchemyTextTrigger":
                    AlchemyTrigger();
                    break;
                case "WorkbenchTextTrigger (1)":
                    WorkbenchTrigger();
                    break;
                case "ExitOutOfCave":
                    OnExitCave();
                    break;
            }
        }

        #region Trigger methods
        /// <summary>
        /// Checks if the player has collected enough ressources to exit the cave and teleport the player to the player cave
        /// </summary>
        private void OnExitCave()
        {
            if (CollectingDone || SkipCollection)
            {
                myFade.FadingWithCallback(1, 1, delegate
                {
                    transform.position = new Vector3(smithEnterObject.transform.position.x,
                        smithEnterObject.transform.position.y, smithEnterObject.transform.position.z);
                    myFade.FadingWithCallback(0, 0.5f, delegate { Debug.Log("Done fading"); });
                    Monolog(playerCaveEntered);

                    //Sound - trigger voiceline 4
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 4", Player.Singleton.transform.position);
                });

                tutorialCave.SetActive(false);
            }
            else
            {
                Monolog(tryExitWithoutMaterials);

                //Sound - triger voiceline 3
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 3", Player.Singleton.transform.position);
            }
        }

        /// <summary>
        /// Activates the alchemy monolog text
        /// </summary>
        private void AlchemyTrigger()
        {
            if (!AlchemyDone)
            {
                Monolog(playerCaveCraftAlchemy);

                //Sound - trigger voiceline 5
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 5", Player.Singleton.transform.position);
            }
        }

        /// <summary>
        /// Activates the workbench monolog text
        /// </summary>
        private void WorkbenchTrigger()
        {
            if (!WorkbenchDone)
            {
                Monolog(playerCaveCraftDismantel);

                //Sound - trigger voiceline 7
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 7", Player.Singleton.transform.position);
            }
        }

        /// <summary>
        /// Activates the smith monolog text
        /// </summary>
        private void SmithTrigger()
        {
            if (!SmithDone)
            {
                Monolog(playerCaveCraftSmith);

                //Sound - trigger voiceline 6
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Voicelines/Line 6", Player.Singleton.transform.position);
            }
        }
        #endregion
    }
}
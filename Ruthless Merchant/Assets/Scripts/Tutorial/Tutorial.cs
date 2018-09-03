using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial Singleton;
        public bool isTutorial;

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

        [SerializeField,Tooltip("Drag Monster from tutorial there")]
        private Monster monster;

        [SerializeField]
        [Tooltip("Drag DefaultSword Prefab there")]
        private Item defaultSword;
        [SerializeField]
        [Tooltip("Drag IronSword Prefab there")]
        private Item ironSword;
        [SerializeField]
        [Tooltip("Drag teleportCaveUp from Tutorial there")]
        private GameObject teleportCaveUp;

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
        private GameObject smithExit;
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


        //Dialogue colliders
        private Collider dialogueZone11Collider;

        private bool SmithDone, WorkbenchDone, AlchemyDone, DismentalDone, CollectingDone;

        private string startMonolog = "'Some call it a shattered world … and I call it home.Not because of my origin.But because I strive to own this place.I can move around with[W][A][S][D], jump with[SPACE], run with[SHIFT], look around by moving the mouse and collect items or interact by pressing[E]. Now I should collect some materials.'";
        private string tryExitWithoutMaterials = "'I think I should collect some materials before I leave'";
        private string collectedMaterials = "'I have enough materials a should go to my cave'";
        private string playerCaveEntered = "'Weird. I prefer dreaming about the future, not the past. Well, it looks like I am not the only one taking a nap. Let’s give my smith some work'";
        private string playerCaveCraftAlchemy = "'At alchemy stations like this one, I can brew potions to sell. I just put herbs in the small bottles and combine their effects with the tools on the table.'";
        private string playerCaveCraftSmith = "'A capable man. I only have to bring him materials and i would craft whatever weapon I need'";
        private string playerCaveCraftDismantel = "'Sometimes, I need to dismantle some of my old stock. This workbench is the perfect tool to do so, and still retrieve some materials.'";
        private string playerCaveFinished = "'Everything is set and prepared. I should leave this place and make some profit by selling my carefully crafted goods'";
        private string TradingInit = "'I offered a pack of 5 Iron Swords. I knew the value of a single one was 29 Gold. We would haggle over it.'";
        private string TradingDoBet = "'He had his own price in mind, probably way lower than I wished. So, I had the choice between pretending a higher value and hopefully selling it to him, or starting lower to please him'";
        private string TradeGoodBet = "'I preferred the chance for profit. He had a sense for that and replied with a lower initial offer. Now, we had to approach each other until we came to an agreement – or someone dropped the whole deal'";
        private string TradeBadBet = "'I opened in a moderate fashion. I could go only lower from there'";
        private string TradeGoodResult = "'I made a profitable deal by selling it for a good price. I should quickly move to the nearby cave.'";
        private string TradeBadResult = "'I may have missed on that price, but a sold item is still a sold item I don’t have to occupy bag space for. And my customer seemed happy. I should quickly move to the nearby cave.'";
        private string TradeIrritationResultGreedy = "'I was to greedy. I expected too much from this deal. Honestly, I would have stopped there by myself. I should quickly move to the nearby cave.'";
        private string TradeIrritationResultNaiv = "'I was still naive and eased the price way to easy. No wonder he couldn’t take me serious anymore. I should quickly move to the nearby cave.'";
        private string TradeFinished = "'Trade finished (monster)'";

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

            myFade.FadingWithCallback(1, 0.001f, delegate {Monolog(startMonolog); myFade.FadingWithCallback(0, 3, delegate { Debug.Log("Done fading"); });
            });

        }

        // Update is called once per frame
        void Update()
        {
            CheckCollection();
            OpenSmithDoor();
            Debug.Log("Smith" + SmithDone + "Alchemy" + AlchemyDone + "Workbench" + WorkbenchDone);
        }

        private void CheckCollection()
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

            bool prevDone = CollectingDone;
            CollectingDone = collectedPlants >= minCollectedPlants && collectedVains >= minCollectedVains;

            if(!prevDone && CollectingDone)
            {
                Monolog(Singleton.collectedMaterials);
            }
        }

        #region TradeMonologs
        public static void TraderMonologStart()
        {
            Monolog(Singleton.TradingInit);
            Singleton.trader.Interact(Player.Singleton.gameObject);
        }

        public static void TraderItemSelectionMonolog2()
        {
            Monolog(Singleton.startMonolog);
        }

        public static void TraderMonolog3()
        {
            Monolog(Singleton.TradingDoBet);
        }

        public static void TraderMonolog4()
        {
            Monolog(Singleton.TradeGoodBet);
        }

        public static void TradeMonolog5()
        {
            Monolog(Singleton.TradeBadBet);
        }

        public static void TradeMonolog6()
        {
            Monolog(Singleton.TradeGoodResult);
        }

        public static void TraderMonolog7()
        {
            Monolog(Singleton.TradeBadResult);
        }

        public static void TraderMonolog8()
        {
            Monolog(Singleton.TradeIrritationResultGreedy);
        }

        public static void TraderMonolog9()
        {
            Monolog(Singleton.TradeIrritationResultNaiv);
        }
        #endregion

        public static void Monolog(string text)
        {
            if (Singleton != null && Singleton.isTutorial && Singleton.textMesh != null)
            {
                Singleton.textMesh.text = text;
                Singleton.textMesh.transform.parent.gameObject.SetActive(true);
            }
        }

        public void OpenSmithDoor()
        {
            if (WorkbenchDone && AlchemyDone && SmithDone)
            {
                int swords = Player.Singleton.Inventory.GetNumberOfItems(ironSword);
                if(swords < 5)
                    Player.Singleton.Inventory.Add(ironSword, 5-swords, true);
                Monolog(playerCaveFinished);
                smithExit.SetActive(false);   
            }        
        }

        public void SmithIsCompleted()
        {
            SmithDone = true;
        }

        public void AlchemyIsCompleted()
        {
            AlchemyDone = true;
        }

        public void WorkbenchIsCompleted()
        {
            WorkbenchDone = true;
        }

        void OnTriggerEnter(Collider other)
        {          
            switch (other.gameObject.name)
            {
                case "10_TriggerZone":
                    //Monolog(10);
                    //Player.Singleton.Inventory.Remove(ironSword, true);
                    break;
                case "13_TriggerZone":
                    //Monolog(13);
                    //Destroy(other.gameObject);
                    break;
                case "TriggerCancelTutorial":
                    TutorialObject.SetActive(false);                
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

        private void OnExitCave()
        {
            if (CollectingDone)
            {
                myFade.FadingWithCallback(1, 1, delegate
                {
                    transform.position = new Vector3(smithEnterObject.transform.position.x,
                        smithEnterObject.transform.position.y, smithEnterObject.transform.position.z);
                    myFade.FadingWithCallback(0, 0.5f, delegate { Debug.Log("Done fading"); });
                    Monolog(playerCaveEntered);
                });
            }
            else
            {
                Monolog(tryExitWithoutMaterials);
            }
        }

        #region PlayerCaveMonologs
        private void AlchemyTrigger()
        {
            if (!AlchemyDone)
            {
                Monolog(playerCaveCraftAlchemy);
            }
        }

        private void WorkbenchTrigger()
        {
            if (!DismentalDone)
            {
                Monolog(playerCaveCraftDismantel);
            }
        }

        private void SmithTrigger()
        {
            if (!SmithDone)
            {
                Monolog(playerCaveCraftSmith);
            }
        }
        #endregion
    }
}

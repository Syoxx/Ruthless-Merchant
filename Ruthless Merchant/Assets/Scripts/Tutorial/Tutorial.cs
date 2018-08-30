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

        //Check bool to start the movement of Monster
        [System.NonSerialized]
        public bool TradeIsDone;

        [SerializeField]
        [Tooltip("Drag IronSword Prefab there")]
        private Item ironSword;
        [SerializeField]
        [Tooltip("Drag teleportCaveUp from Tutorial there")]
        private GameObject teleportCaveUp;
        [SerializeField]
        [Tooltip("Drag Enter from Tutorial there")]
        private GameObject caveEnter;
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
        [SerializeField]
        [Tooltip("Drag Monster from tutorial there")]
        private GameObject TutorialMonster;

        private Collider playerCollider, teleportUpCollider, exitOutOfCaveCollider;
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

        private bool SmithDone, WorkbenchDone, AlchemyDone;


        //Text Monolog
        private int speechCounter;
        private string[] MonologSpeech =
        {
            "'I offered a pack of 5 Iron Swords. I knew the value of a single one was 29 Gold. We would haggle over it.'",                                                                                                                                           //0 Start while fading
            "'I offered a pack of 5 Iron Swords. I knew the value of a single one was 29 Gold. We would haggle over it.'",                                                                                                                                           //1 Start at trade                                         
            "'He had his own price in mind, probably way lower than I wished. So, I had the choice between pretending a higher value and hopefully selling it to him, or starting lower to please him'",                                                            //2 Start "Angebot"
            "'I preferred the chance for profit. He had a sense for that and replied with a lower initial offer. Now, we had to approach each other until we came to an agreement – or someone dropped the whole deal'",                                            //3 Nach Setzen des Startangebotes, sofern nicht direkt abbricht, Höher als Realwert
            "'I opened in a moderate fashion. I could go only lower from there'",                                                                                                                                                                                   //4 Niedriger als Realwert
            "'I made a profitable deal by selling it for a good price. I should quickly move to the nearby cave.'",                                                                                                                                                                                           //5 Beim Ende Wenn über oder genau bei Realwert verkauft
            "'I may have missed on that price, but a sold item is still a sold item I don’t have to occupy bag space for. And my customer seemed happy. I should quickly move to the nearby cave.'",                                                                                                          //6 Beim Ende Wenn unter Realwert verkauft: 
            "'I was to greedy. I expected too much from this deal. Honestly, I would have stopped there by myself. I should quickly move to the nearby cave.'",                                                                                                                                               //7 Beim Ende Wenn wegen Genervtheit 
            "'I was still naive and eased the price way to easy. No wonder he couldn’t take me serious anymore. I should quickly move to the nearby cave.'",                                                                                                                                                   //8 Beim Ende Wenn wegen Genervtheit     
            "'I quickly escaped to the nearby cave. However, their fortune was different. It was the first time I encountered one of those … things. Not exactly my audience.'",  //9 Monster Appears
            "'There was only one way out, a hole in the floor. I had to jump …'",                                                                                                                                                                                                        //10 Enter Cave
            "'… and landed in a structure underneath. There were some obstacles, but nothing to worry about. The exit couldn’t be far away'",                                                                                                                       //11 Bottom Cave
            "'Weird. I prefer dreaming about the future, not the past. Well, it looks like I am not the only one taking a nap. Let’s give my smith some work'",                                                                                                     //12 Waking Up
            "'A capable man. I only have to bring him materials and i would craft whatever weapon I need'",                                                                                                                                                         //13 At Smith
            "'Everything is set and prepared. I should leave this place and make some profit by selling my carefully crafted goods'",                                                                                                                                                                                                                       //14 After Smith
            "'Sometimes, I need to dismantle some of my old stock. This workbench is the perfect tool to do so, and still retrieve some materials.'",
            "'At alchemy stations like this one, I can brew potions to sell. I just put herbs in the small bottles and combine their effects with the tools on the table.'",
            "'Everything is set and prepared. I should leave this place and make some profit by selling my carefully crafted goods.'"
        };
        // Hardcoded Stuff

        private void Awake()
        {
            Singleton = this;
        }

        void Start()
        {
            textMesh = textMesh.GetComponent<TextMeshProUGUI>();
            playerCollider = PlayerObject.GetComponent<Collider>();
            teleportUpCollider = teleportCaveUp.GetComponent<Collider>();
            exitOutOfCaveCollider = exitOutOfCaveObject.GetComponent<Collider>();

            // Dialogues
            dialogueZone11Collider = dialogueZone10.GetComponent<Collider>();


            Player.Singleton.Inventory.Add(ironSword, 5, true);

            myFade.FadingWithCallback(1, 0.001f, delegate {Monolog(0); myFade.FadingWithCallback(0, 3, delegate { Debug.Log("Done fading"); });
            });

        }

        // Update is called once per frame
        void Update()
        {
            Teleports();
            OpenSmithDoor();
        } 

        public static void Monolog(int speechIndex)
        {
            if (Singleton != null && Singleton.isTutorial && Singleton.textMesh != null)
            {
                Singleton.textMesh.text = Singleton.MonologSpeech[speechIndex];
                Singleton.textMesh.transform.parent.gameObject.SetActive(true);
                Debug.Log("Monolog " + speechIndex);
            }
        }

        public void OpenSmithDoor()
        {
            if (WorkbenchDone && AlchemyDone && WorkbenchDone)
            {
                smithExit.SetActive(false);   
            }        
        }
        private void Teleports()
        {
            if (playerCollider.bounds.Intersects(teleportUpCollider.bounds))
            {
                myFade.FadingWithCallback(1, 1, delegate
                {
                    //transform.position = new Vector3(caveEnter.transform.position.x, caveEnter.transform.position.y - 5,
                    //  caveEnter.transform.position.z);
                    //transform.position = caveEnter.transform.position;
                    Vector3 teleportTarget = new Vector3(386.635f, 7.847f, 68.289f) ;
                    //teleportTarget.y = teleportTarget.y - 4;
                    myFade.FadingWithCallback(0, 0.5f, delegate { transform.position = teleportTarget; });
                });

                Monolog(11);
            }

            if (playerCollider.bounds.Intersects(exitOutOfCaveCollider.bounds))
            {
                myFade.FadingWithCallback(1, 1, delegate
                {
                    transform.position = new Vector3(smithEnterObject.transform.position.x,
                        smithEnterObject.transform.position.y, smithEnterObject.transform.position.z);
                    myFade.FadingWithCallback(0, 0.5f, delegate { Debug.Log("Done fading"); });
                });

                Monolog(12);
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
                    Monolog(10);
                    TutorialMonster.SetActive(false);
                    break;
                case "13_TriggerZone":
                    Monolog(13);
                    Destroy(other.gameObject);
                    break;
                case "TriggerCancelTutorial":
                    TutorialObject.SetActive(false);
                    Player.Singleton.Inventory.Remove(ironSword, true);
                    break;
                case "AlchemyTextTrigger":
                    Monolog(16);
                    break;
                case "WorkbenchTextTrigger (1)":
                    Monolog(15);
                    break;
            }
        }
    }
 

}

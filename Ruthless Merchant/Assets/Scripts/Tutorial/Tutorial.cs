using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Item ironSword;
        [SerializeField] private GameObject teleportCaveUp;
        [SerializeField] private GameObject caveEnter;
        [SerializeField] private GameObject PlayerObject;
        [SerializeField] private GameObject exitOutOfCaveObject;
        [SerializeField] private GameObject smithEnterObject;
        [SerializeField] private GameObject smithExit;
        [SerializeField] private TextMeshProUGUI textMesh;
        private Collider playerCollider, teleportUpCollider, exitOutOfCaveCollider;
        [SerializeField] private Image myFade;


        //Text Monolog
        private int speechCounter;
        private string[] MonologSpeech =
        {
            "'Some call it a shattered world … and I call it home. Not because of my origin. But because I strive to own this place. I remember my first deal like it was yesterday. In piece, they preferred a little privacy.'",                                  //0 Start while fading
            "'I offered a pack of Iron Swords. I knew the value of a single one was 29 Gold. We would haggle over it.'",                                                                                                                                            //1 Start at trade                                         
            "'He had his own price in mind, probably way lower than I wished. So, I had the choice between pretending a higher value and hopefully selling it to him, or starting lower to please him'",                                                            //2 Start "Angebot"
            "'I preferred the chance for profit. He had a sense for that and replied with a lower initial offer. Now, we had to approach each other until we came to an agreement – or someone dropped the whole deal'",                                            //3 Nach Setzen des Startangebotes, sofern nicht direkt abbricht, Höher als Realwert
            "'I opened in a moderate fashion. I could go only lower from there'",                                                                                                                                                                                   //4 Niedriger als Realwert
            "'I made a profitable deal by selling it for a good price.'",                                                                                                                                                                                           //5 Beim Ende Wenn über oder genau bei Realwert verkauft
            "'I may have missed on that price, but a sold item is still a sold item I don’t have to occupy bag space for. And my customer seemed happy.'",                                                                                                          //6 Beim Ende Wenn unter Realwert verkauft: 
            "'I was to greedy. I expected too much from this deal. Honestly, I would have stopped there by myself.'",                                                                                                                                               //7 Beim Ende Wenn wegen Genervtheit 
            "'I was still naive and eased the price way to easy. No wonder he couldn’t take me serious anymore'",                                                                                                                                                   //8 Beim Ende Wenn wegen Genervtheit     
            "'It was time for me to return. However, their fortune was different. It was the first time I encountered one of those … things. Not exactly my audience. I may have watched the slaughter for some time, but I quickly made it to the nearby cave.'",  //9 Monster Appears
            "'There was only one way out. I had to jump …'",                                                                                                                                                                                                        //10 Enter Cave
            "'… and landed in a structure underneath. There were some obstacles, but nothing to worry about. The exit couldn’t be far away'",                                                                                                                       //11 Bottom Cave
            "'Weird. I prefer dreaming about the future, not the past. Well, it looks like I am not the only one taking a nap. Let’s give my smith some work'",                                                                                                     //12 Waking Up
            "'A capable man. I only have to bring him materials and he would craft whatever weapon I need'",                                                                                                                                                        //13 At Smith
            "Let’s go. It’s business hour"                                                                                                                                                                                                                          //14 After Smith
        };
        // Hardcoded Stuff
        void Start()
        {
            textMesh = textMesh.GetComponent<TextMeshProUGUI>();
            playerCollider = PlayerObject.GetComponent<Collider>();
            teleportUpCollider = teleportCaveUp.GetComponent<Collider>();
            exitOutOfCaveCollider = exitOutOfCaveObject.GetComponent<Collider>();
            Player.Singleton.Inventory.Add(ironSword, 5, true);

            myFade.FadingWithCallback(1, 0.001f, delegate {Monolog(0); myFade.FadingWithCallback(0, 3, delegate { Debug.Log("Done fading"); });
            });

        }

        // Update is called once per frame
        void Update()
        {
            Teleports();
        }

        private void Monolog(int speechIndex)
        {
            textMesh.text = MonologSpeech[speechCounter];
        }

        public void OpenSmithDoor()
        {
            smithExit.SetActive(false);
        }
        private void Teleports()
        {
            if (playerCollider.bounds.Intersects(teleportUpCollider.bounds))
            {
                myFade.FadingWithCallback(1, 1, delegate
                {
                    transform.position = new Vector3(caveEnter.transform.position.x, caveEnter.transform.position.y - 5,
                        caveEnter.transform.position.z);
                    myFade.FadingWithCallback(0, 1, delegate { Debug.Log("Done fading"); });
                });
            }

            if (playerCollider.bounds.Intersects(exitOutOfCaveCollider.bounds))
            {
                myFade.FadingWithCallback(1, 1, delegate
                {
                    transform.position = new Vector3(smithEnterObject.transform.position.x,
                        smithEnterObject.transform.position.y, smithEnterObject.transform.position.z);
                    myFade.FadingWithCallback(0, 1, delegate { Debug.Log("Done fading"); });
                });
            }
        }
    }
 

}

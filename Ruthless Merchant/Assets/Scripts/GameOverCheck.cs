using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    /// <summary>
    /// Checks if all Outposts are captured by one faction
    /// </summary>
    public class GameOverCheck : MonoBehaviour
    {
        [SerializeField, Range(0, 60)]
        private float duration = 10;

        [SerializeField, Tooltip("Time it takes to fade to Game Over Screen")]
        private float fadeDuration = 1f;

        [SerializeField, Tooltip("GameOver Screen for Imperialist win")]
        private Image imperialWinImg;

        [SerializeField, Tooltip("GameOver Screen for Freemind win")]
        private Image freemindsWinImg;

        private Image winningFactionImg;
        private bool gameOver = false;
        private float elapsedTime = 0;

        // Update is called once per frame
        void Update()
        {
            if (!gameOver)
            {
                if ((CaptureTrigger.OwnerStatistics[Faction.Imperialisten] <= 0 || CaptureTrigger.OwnerStatistics[Faction.Freidenker] <= 0) && CaptureTrigger.OwnerStatistics[Faction.Neutral] == 0)
                {
                    gameOver = true;

                    if (CaptureTrigger.OwnerStatistics[Faction.Imperialisten] <= 0)
                        winningFactionImg = freemindsWinImg;
                    if (CaptureTrigger.OwnerStatistics[Faction.Freidenker] <= 0)
                        winningFactionImg = imperialWinImg;

                    winningFactionImg.FadingWithCallback(fadeDuration, 1f, delegate { Debug.Log("Game Over"); });
                }
            }
            else
            {
                elapsedTime += Time.deltaTime;
                if(elapsedTime >= duration)
                {
                    SceneManager.LoadScene("NewMainMenu");
                }
            }
        }
    }
}

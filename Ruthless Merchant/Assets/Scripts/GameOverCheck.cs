using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{
    /// <summary>
    /// Checks if all Outposts are captured by one faction
    /// </summary>
    public class GameOverCheck : MonoBehaviour
    {
        [SerializeField, Range(0, 60)]
        private float duration = 10;

        private bool gameOver = false;
        private float elapsedTime = 0;

        // Update is called once per frame
        void Update()
        {
            if (!gameOver)
            {
                if ((CaptureTrigger.OwnerStatistics[Faction.Imperialisten] <= 0 || CaptureTrigger.OwnerStatistics[Faction.Freidenker] <= 0) && (!CaptureTrigger.OwnerStatistics.ContainsKey(Faction.Neutral) || CaptureTrigger.OwnerStatistics[Faction.Neutral] == 0))
                {
                    gameOver = true;
                    //TODO show gameover image
                    Debug.Log("GameOver");
                }
            }
            else
            {
                elapsedTime += Time.deltaTime;
                if(elapsedTime >= duration)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }
}

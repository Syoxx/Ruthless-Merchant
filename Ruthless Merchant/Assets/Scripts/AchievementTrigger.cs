using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RuthlessMerchant {
    public class AchievementTrigger : MonoBehaviour {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && Achievements.Singleton.switchIndex == 2)
            {
                Achievements.AddToCounter();
                Destroy(gameObject);
            }

    }
    } }

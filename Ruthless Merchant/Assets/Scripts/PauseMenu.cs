using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuthlessMerchant
{
    public class PauseMenu : MonoBehaviour
    {
        private GameSettings settings;
        public GameSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
            }
        }
    }
}
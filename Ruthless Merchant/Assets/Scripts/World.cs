using System;

namespace RuthlessMerchant
{
    public class World
    {
        private TimeSpan time;
        private InteractiveObject[] interactiveObjects;
        private UISystem uiSystem;
        private DialogSystem dialogSystem;
        private GameSettings settings;

        public InteractiveObject[] InteractiveObjects
        {
            get
            {
                return interactiveObjects;
            }
            set
            {
                interactiveObjects = value;
            }
        }

        public UISystem UISystem
        {
            get
            {
                return uiSystem;
            }
            set
            {
                uiSystem = value;
            }
        }

        public DialogSystem DialogSystem
        {
            get
            {
                return dialogSystem;
            }
            set
            {
                dialogSystem = value;
            }
        }

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

        public RuthlessMerchant.VictoryPoint[] VictoryPoints
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public RuthlessMerchant.Building[] Buildings
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public void CheckPlayerState()
        {
            throw new System.NotImplementedException();
        }

        public void CheckFactionState()
        {
            throw new System.NotImplementedException();
        }

        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        public void Resume()
        {
            throw new System.NotImplementedException();
        }
    }
}
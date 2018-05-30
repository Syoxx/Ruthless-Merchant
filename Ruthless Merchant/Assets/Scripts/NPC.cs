using System;

namespace RuthlessMerchant
{
    public class NPC : Character
    {
        private DayCycle dayCycle;
        private DialogSystem dialogSystem;
        private int fov;
        private int viewDistance;
        private int attackDistance;

        public event EventHandler OnCharacterNoticed;
        public event EventHandler OnItemNoticed;
        public event EventHandler OnHeardSomething;

        public DayCycle DayCycle
        {
            get
            {
                return dayCycle;
            }
            set
            {
                dayCycle = value;
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

        public override void Interact()
        {
            throw new System.NotImplementedException();
        }

        public void OpenDialog()
        {
            throw new System.NotImplementedException();
        }

        public void Sleep()
        {
            throw new System.NotImplementedException();
        }

        public void Flee()
        {
            throw new System.NotImplementedException();
        }

        public void Hear()
        {
            throw new System.NotImplementedException();
        }
    }
}
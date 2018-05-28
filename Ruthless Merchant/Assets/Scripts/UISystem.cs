using System;

namespace RuthlessMerchant
{
    public class UISystem
    {
        public event EventHandler OnUIOpened;
        public event EventHandler OnUIClosed;

        private UI[] uis;
        private UI currentUI;

        public UI[] UIs
        {
            get
            {
                return uis;
            }
            set
            {
                uis = value;
            }
        }

        public UI CurrentUI
        {
            get
            {
                return currentUI;
            }
            set
            {
                currentUI = value;
            }
        }
    }
}
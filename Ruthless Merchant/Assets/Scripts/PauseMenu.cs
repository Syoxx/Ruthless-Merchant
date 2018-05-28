namespace RuthlessMerchant
{
    public class PauseMenu : UI
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
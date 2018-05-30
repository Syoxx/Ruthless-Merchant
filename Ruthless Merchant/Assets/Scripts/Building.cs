namespace RuthlessMerchant
{
    public abstract class Building
    {
        private bool productionEnabled;
        private float productionDuration;
        private float elapsedProductionTime;
        private Faction faction;
        private bool loopProduction;

        public Faction Faction
        {
            get { return Faction; }
        }

        public void UpdateProduction()
        {
            throw new System.NotImplementedException();
        }

        public abstract void OnProductionCompleted();

        public void PauseProduction()
        {
            throw new System.NotImplementedException();
        }

        public void ResumeProduction()
        {
            throw new System.NotImplementedException();
        }

        public void AbortProduction()
        {
            throw new System.NotImplementedException();
        }
    }
}
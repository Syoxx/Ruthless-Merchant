namespace RuthlessMerchant
{
    public class GearSystem
    {
        private GearSlot[] gearSlots;
        private StatValue[] totalGearStats;

        public GearSlot[] GearSlots
        {
            get
            {
                return gearSlots;
            }
            set
            {
                gearSlots = value;
            }
        }

        public StatValue[] TotalGearStats
        {
            get
            {
                return totalGearStats;
            }
            set
            {
                totalGearStats = value;
            }
        }

        public void UpdateGearStats()
        {
            throw new System.NotImplementedException();
        }
    }
}
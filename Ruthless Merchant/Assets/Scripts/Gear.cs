namespace RuthlessMerchant
{
    public class Gear : Item
    {
        private StatValue[] stats;
        private GearSlotType gearSlotType;

        public StatValue[] Stats
        {
            get
            {
                return stats;
            }
            set
            {
                stats = value;
            }
        }

        public GearSlotType GearSlotType
        {
            get
            {
                return gearSlotType;
            }
            set
            {
                gearSlotType = value;
            }
        }
    }
}
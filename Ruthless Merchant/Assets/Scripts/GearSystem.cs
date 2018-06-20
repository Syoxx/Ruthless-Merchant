namespace RuthlessMerchant
{
    public class GearSystem
    {
        private GearSlot[] gearSlots;
        private StatValue[] totalGearStats;
        private GearSlot ProtectiveGear, Weaponry;

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

        public GearSystem(bool isPlayer)
        {           
            if (!isPlayer)
            {
                ProtectiveGear.GearSlotType = GearSlotType.ArmorSlot;
                Weaponry.GearSlotType = GearSlotType.WeaponSlot;
                GearSlot[] gearSlots = new GearSlot[] { ProtectiveGear, Weaponry };
            }
        }

        public void EquipGear(Item GearItem)
        {
            if (GearItem.Type == ItemType.Weapon)
            {
                // Remove reference to gear in GearSlot
                if (ProtectiveGear.Gear != null)
                {
                    ProtectiveGear.Gear = null;
                }

                // Add GearItem
                ProtectiveGear.Gear = GearItem;
            }
            else if (GearItem.Type == ItemType.Gear)
            {

            }
            // UpdateGearStats
        }

        public void UpdateGearStats()
        {
            //TODO: GearStats
            throw new System.NotImplementedException();
        }
    }
}
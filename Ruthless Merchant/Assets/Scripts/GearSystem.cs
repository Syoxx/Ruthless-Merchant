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
            if (GearItem.Type == ItemType.Gear)
            {
                // Remove reference to gear in GearSlot
                if (ProtectiveGear.Gear != null)
                {
                    ProtectiveGear.Gear = null;
                }

                Gear CastedItem = GearItem as Gear;

                // Add GearItem
                if (CastedItem != null)
                    ProtectiveGear.Gear = CastedItem;
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
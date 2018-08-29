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
            if (GearItem.ItemInfo.ItemType == ItemType.Gear)
            {
                // Remove reference to gear in GearSlot
                if (ProtectiveGear.Gearitem != null)
                {
                    ProtectiveGear.Gearitem = null;
                }

                Gear CastedGear = GearItem as Gear;

                // Add GearItem
                if (CastedGear != null)
                    ProtectiveGear.Gearitem = CastedGear;
            }

            if (GearItem.ItemInfo.ItemType == ItemType.Weapon)
            {
                // Remove reference to gear in GearSlot
                if (Weaponry.Gearitem != null)
                {
                    ProtectiveGear.Gearitem = null;
                }

                Weapon CastedWeapon = GearItem as Weapon;

                // Add GearItem
                if (CastedWeapon != null)
                    Weaponry.Gearitem = CastedWeapon;
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
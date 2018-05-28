namespace RuthlessMerchant
{
    public class Weapon : Item
    {
        private float attackRate;
        private int damage;
        private int range;
        private WeaponType type;
        private Item ammo;

        public Item Ammo
        {
            get
            {
                return ammo;
            }
            set
            {
                ammo = value;
            }
        }
    }
}
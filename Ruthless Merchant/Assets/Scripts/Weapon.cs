using UnityEditor;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Weapon : Item
    {   
        //Made by Daniil Masliy

        [Header("Weapon Parameters")]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private int damage;

        [SerializeField] private int defencePower;
        [SerializeField] private float attackSpeed;

        public WeaponType WeaponType
        {
            get
            {
                return weaponType;
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }
        }

        public int DefencePower
        {
            get
            {
                return defencePower;
            }
        }

        public float AttackSpeed
        {
            get
            {
                return attackSpeed;
            }
        }

        public override void Start()
        {
            
        }

        public override void Update()
        {
           
        }
    }


}
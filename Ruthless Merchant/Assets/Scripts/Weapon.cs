using UnityEditor;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Weapon : Item
    {   
        //Made by Daniil Masliy

        [Header("Weapon Parameters")]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private int Damage;

        [SerializeField] private int DefencePower;
        [SerializeField] private int AttackSpeed;

        public override void Start()
        {
            
        }

        public override void Update()
        {
           
        }
    }


}
using UnityEditor;
using UnityEngine;
using UnityEditor;

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


        [Header("Crating")]
        [SerializeField] private MaterialsType firstIngredient;
        [SerializeField] private int firstIngredientAmount;
        [SerializeField] private MaterialsType secondIngredient;
        [SerializeField] private int secondIngredientAmount;
        [SerializeField] private MaterialsType thirdIngredient;
        [SerializeField] private int thirdIngredientAmount;

        public override void Start()
        {
            
        }

        public override void Update()
        {
           
        }
    }


}
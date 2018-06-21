using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class Potion : Item
    {
        [Header("Potion Effects (Nur zum testen!")]
        [SerializeField] private int Angriffgeschwindigkeit;
        [SerializeField] private int Verteidigung;
        [SerializeField] private int Laufgeschwindigkeit;
        [SerializeField] private int MaximaleTP;
        [SerializeField] private int Heilung;



        public override void Start()
        {

        }

        public override void Update()
        {

        }
    }

}


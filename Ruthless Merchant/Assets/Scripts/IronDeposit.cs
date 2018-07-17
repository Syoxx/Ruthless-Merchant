using UnityEngine;
using System;

namespace RuthlessMerchant
{

    public class IronDeposit : InteractiveWorldObject
    {
        #region Private Fields
        [SerializeField]
        [Tooltip("Insert Item Prefab for the desired spawned Item here")]
        private GameObject ironPrefab;

        [SerializeField]
        [Tooltip("Insert Item Prefab for the desired spawned Rare Item here")]
        private GameObject rareItemPrefab;

        [SerializeField]
        [Tooltip("Check to enable Rare Item Drop")]
        private bool allowRareItemDrop = false;

        [SerializeField]
        private float forceMultiplier = 1;

        [Header("Irons in Deposits")]
        [Tooltip("Number of Irons in Deposit will be Randomized between these to Variables")]
        [SerializeField]
        private int minNrOfIron = 1, maxNrOfIron = 3;

        [Header("Rare Items")]
        [SerializeField]
        [Tooltip("Chance to Drop a rare Item")]
        [Range(0, 100)]
        private float rareItemDropChance = 20;


        private int numberOfIrons;
        private Transform spawnPosition;
        private Vector3 ironForce;
        private System.Random rnJesus;
        #endregion



        #region Gameplay Loop
        // Use this for initialization
        public override void Start()
        {
            rnJesus = new System.Random();
            numberOfIrons = rnJesus.Next(minNrOfIron, maxNrOfIron);
            ironForce.x = 100f;
            ironForce.z = 100f;
            ironForce.y = 1f;
        }

        // Update is called once per frame
        public override void Update()
        {

        }
        #endregion

        #region Methods
        public override void Interact(GameObject caller)
        {
            DropItems();
        }

        /// <summary>
        /// Initiates Spawning of dropped Items and Rare Items
        /// Destroys GameObject afterwards
        /// </summary>
        private void DropItems()
        {
            if (allowRareItemDrop)
                DropRareItem();

            for (int i = 1; i <= numberOfIrons; i++)
            {
                SpawnItem(ironPrefab, true, i);
                if (i == numberOfIrons)
                    Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Spawns an Item at the current Position and adds a Force to it
        /// </summary>
        /// <param name="itemPrefab">prefab which is spawned</param>
        /// <param name="isIron">if its an iron prefab</param>
        /// <param name="nrOfItem">the number if items spawned</param>
        private void SpawnItem(GameObject itemPrefab, bool isIron, int nrOfItem)
        {
            spawnPosition = this.transform;
            GameObject spawnedIron = Instantiate(itemPrefab,spawnPosition.position, spawnPosition.rotation);
            spawnedIron.GetComponent<Rigidbody>().AddForce(CalculateForce(isIron, nrOfItem));
        }

        /// <summary>
        /// Calculates if a Rare Item Drops and Initiates Spawning
        /// </summary>
        private void DropRareItem()
        {
            int randomRoll = rnJesus.Next(0, 100);
            if (randomRoll <= rareItemDropChance)
                SpawnItem(rareItemPrefab, false, 1);
        }

        private Vector3 CalculateForce(bool isIron, int nrOfItem)
        {
            ironForce = ironForce * forceMultiplier;
            ironForce = RotateForceVector(ironForce, nrOfItem);
            return ironForce;
        }

        private Vector3 RotateForceVector(Vector3 inputVector, int nrOfItem)
        {
            int randomEuler;
            Vector3 rotatedVector;
            Vector3 directionVector;
            float xForce = 75f;
            float zForce = 75f;
            float yForce = 125f;
            switch (nrOfItem)
            {
                case 1:
                    randomEuler = 0;
                    rotatedVector.x = xForce;
                    rotatedVector.z = zForce;
                    rotatedVector.y = yForce;
                    directionVector = Vector3.right;
                    break;
                case 2:
                    randomEuler = 90;
                    rotatedVector.x = -xForce;
                    rotatedVector.z = zForce;
                    rotatedVector.y = yForce;
                    directionVector = Vector3.left;
                    break;
                case 3:
                    randomEuler = 180;
                    rotatedVector.x = xForce;
                    rotatedVector.z = -zForce;
                    rotatedVector.y = yForce;
                    break;
                case 4:
                    randomEuler = 270;
                    rotatedVector.x = -xForce;
                    rotatedVector.z = -zForce;
                    rotatedVector.y = yForce;
                    break;
                default:
                    randomEuler = 360;
                    rotatedVector.x = xForce;
                    rotatedVector.z = zForce;
                    rotatedVector.y = yForce;
                    break;
            }
            rotatedVector = Quaternion.AngleAxis(45, Vector3.forward) * rotatedVector;
            Debug.Log(rotatedVector);
            return rotatedVector;
        }
        #endregion
    }
}

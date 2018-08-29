using UnityEngine;

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

        [Header("Irons in Deposits")]
        [Tooltip("Number of Irons in Deposit will be Randomized between these to Variables")]
        [SerializeField]
        private int minNrOfIron = 1;
        [SerializeField]
        private int maxNrOfIron = 3;

        [Header("Rare Items")]
        [SerializeField]
        [Tooltip("Chance to Drop a rare Item")]
        [Range(0, 100)]
        private float rareItemDropChance = 20;

        [Header("Forces for Iron Spawn")]
        [SerializeField]
        [Range(50,100)]
        [Tooltip("Force applied in X-Direction")]
        private float xForce = 75f;
        [SerializeField]
        [Range(50, 100)]
        [Tooltip("Force applied in Z-Direction")]
        private float zForce = 75f;
        [SerializeField]
        [Range(100, 150)]
        [Tooltip("Force Applied in Y-Direction")]
        private float yForce = 125f;

        private int numberOfIrons;
        private Transform spawnPosition;
        private Vector3 ironForce;
        private static System.Random rnJesus = new System.Random();
        #endregion



        #region Gameplay Loop
        // Use this for initialization
        public override void Start()
        {
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
            spawnedIron.GetComponent<Rigidbody>().AddForce(RotateForceVector(nrOfItem));
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

        /// <summary>
        /// Randomizes the Force apllied to the Iron Items after spawning
        /// </summary>
        /// <param name="nrOfItem">The numbere of the item currently spawning, used to determin force vector</param>
        /// <returns></returns>
        private Vector3 RotateForceVector(int nrOfItem)
        {
            Vector3 rotatedVector;
            Vector3 directionVector;
            int modOfItem = nrOfItem % 4;
            int randomizedAngle;
            switch (modOfItem)
            {
                case 0:
                    rotatedVector.x = xForce;
                    rotatedVector.z = zForce;
                    rotatedVector.y = yForce;
                    directionVector = Vector3.right;
                    randomizedAngle = rnJesus.Next(-45, 45);
                    break;
                case 1:
                    rotatedVector.x = -xForce;
                    rotatedVector.z = zForce;
                    rotatedVector.y = yForce;
                    directionVector = Vector3.left;
                    randomizedAngle = rnJesus.Next(-45, 45);
                    break;
                case 2:
                    rotatedVector.x = xForce;
                    rotatedVector.z = -zForce;
                    rotatedVector.y = yForce;
                    directionVector = Vector3.back;
                    randomizedAngle = rnJesus.Next(45, 45);
                    break;
                case 3:
                    rotatedVector.x = -xForce;
                    rotatedVector.z = -zForce;
                    rotatedVector.y = yForce;
                    directionVector = new Vector3(-1,0,-1);
                    randomizedAngle = rnJesus.Next(-45, 45);
                    break;
                default:
                    rotatedVector.x = xForce;
                    rotatedVector.z = zForce;
                    rotatedVector.y = yForce;
                    directionVector = Vector3.right;
                    randomizedAngle = rnJesus.Next(-45, 45);
                    break;
            }
            Debug.Log("rnjesus: " + randomizedAngle);
            rotatedVector = Quaternion.AngleAxis(randomizedAngle, directionVector) * rotatedVector;
            return rotatedVector;
        }
        #endregion
    }
}

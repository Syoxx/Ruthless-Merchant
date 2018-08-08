
using UnityEngine;

namespace RuthlessMerchant
{

    public class ContainingItemInformation : MonoBehaviour
    {
        #region Fields
        private GameObject containingGameObject;

        [SerializeField]
        [Tooltip("Put Prefab of the Item which is spawned here, has to be the same as in the Spawn Location Script")]
        private GameObject itemToSpawn;
        #endregion

        #region MonoBehaviour LifeCycle
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        #endregion


        #region Methods
        public bool CheckContainingItem(GameObject otherGameObject)
        {
            if (containingGameObject == null)
                return false;
            else if (containingGameObject.tag == otherGameObject.tag)
                return true;
            else
                return false;
        }
        #endregion

        #region OnTriggerMethods

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == itemToSpawn.tag)
            {
                containingGameObject = other.gameObject;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == itemToSpawn.tag)
            {
                containingGameObject = null;
            }
        }
        #endregion
    }
}

using UnityEngine;

namespace RuthlessMerchant
{

    public class ContainingItemInformation : MonoBehaviour
    {
        private GameObject containingGameObject;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool CheckContainingItem(GameObject otherGameObject)
        {
            if (containingGameObject == null)
                return false;
            else if (containingGameObject.tag == otherGameObject.tag)
                return true;
            else
                return false;
        }

        public void OnTriggerEnter(Collider other)
        {
            containingGameObject = other.gameObject;
        }

        public void OnTriggerExit(Collider other)
        {
            containingGameObject = null;
        }
    }
}
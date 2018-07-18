
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
            if (containingGameObject.tag == otherGameObject.tag)
                return true;
            else
                return false;
        }

        private void OnTriggerStay(Collider other)
        {
            containingGameObject = other.gameObject;
        }
    }
}
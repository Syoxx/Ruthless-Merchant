using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RuthlessMerchant
{
    public class QuestTester : MonoBehaviour
    {
        CollectionGoal collectionGoal;



        [SerializeField]
        Material wood;
        [SerializeField]
        Material iron;

        public float speed = 1.5f;

        public CollectionGoal CollectionGoal
        {
            get
            {
                if (collectionGoal == null)
                {
                    collectionGoal = GetComponent<CollectionGoal>();
                }
                return collectionGoal;
            }
        }
        
        private void Start()
        {
            collectionGoal = GetComponent<CollectionGoal>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0,0,1) * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
            }
        }



        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("NPC"))
        //    {
        //        Debug.Log("Collided");
        //        List<Collectables> collectables = collectionGoal.collectables;
        //        collision.gameObject.AddComponent<CollectionGoal>();
        //        //CollectionGoal collectionGoals = collision.gameObject.GetComponent<CollectionGoal>();
        //        //////collectionGoal = collision.gameObject.AddComponent<CollectionGoal>();
        //        //collectionGoals.FillList(collectables);

        //        collectionGoal = collision.gameObject.GetComponent<CollectionGoal>();
        //        collectionGoal.FillList(collectables);


        //    }
        //}

    }
}

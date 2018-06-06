using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{
    public abstract class NPC : Character
    {
        private DayCycle dayCycle;
        private DialogSystem dialogSystem;

        protected List<Waypoint> waypoints;
        protected int waypointIndex = 0;
        protected Nullable<Waypoint> currentWaypoint = null;

        [SerializeField]
        [Range(0.1f, 100.0f)]
        protected float rotationSpeed = 8.0f;

        [SerializeField]
        [Range(25, 90)]
        protected int fov = 45;

        [SerializeField]
        [Range(1, 1000)]
        protected int viewDistance = 10;

        [SerializeField]
        [Range(1, 1000)]
        protected int hearDistance = 5;

        [SerializeField]
        [Range(1, 1000)]
        protected int attackDistance = 5;

        protected NavMeshAgent agent;
        private Vector3 currentTargetPosition;

        private List<GameObject> possibleSeenObjects;
        private List<AudioSource> possibleHearedObjects;

        private List<GameObject> noticedGameObjects;

        public event EventHandler OnCharacterNoticed;
        public event EventHandler OnItemNoticed;
        public event EventHandler OnHeardSomething;

        private float elapsedWaitTime = 0.0f;
        private Vector3 rotationTarget;

        public override void Start()
        {
            possibleHearedObjects = new List<AudioSource>();
            possibleSeenObjects = new List<GameObject>();
            noticedGameObjects = new List<GameObject>();
            waypoints = new List<Waypoint>();

            GameObject hearObject = transform.GetChild(0).gameObject;
            GameObject seeObject = transform.GetChild(1).gameObject;

            SphereCollider seeCollider = seeObject.GetComponent<SphereCollider>();
            seeCollider.radius = viewDistance;

            SphereCollider hearCollider = hearObject.GetComponent<SphereCollider>();
            hearCollider.radius = hearDistance;

            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            //agent.updateRotation = false;
        }
        
        public DayCycle DayCycle
        {
            get
            {
                return dayCycle;
            }
            set
            {
                dayCycle = value;
            }
        }

        public DialogSystem DialogSystem
        {
            get
            {
                return dialogSystem;
            }
            set
            {
                dialogSystem = value;
            }
        }

        public override void Update()
        {
            Recognize();
            Hear();
            React();

            if (!agent.pathPending && agent.remainingDistance < agent.baseOffset)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                elapsedWaitTime += Time.deltaTime;
                SetDestination();
            }
            else
            {
                RotateToNextTarget(agent.steeringTarget + transform.forward, true);
            }
        }

        private void SetDestination()
        {
            if (waypoints.Count <= 0)
            {
                currentWaypoint = null;
                return;
            }

            Waypoint waypoint = waypoints[waypointIndex];
            if (elapsedWaitTime >= waypoint.WaitTime - 1.0f)
                RotateToNextTarget(waypoint.GetPosition(), false);

            if (elapsedWaitTime >= waypoint.WaitTime)
            {
                elapsedWaitTime = 0.0f;
                agent.SetDestination(waypoint.GetPosition());
                agent.isStopped = false;
                agent.updateRotation = false;
                currentWaypoint = waypoint;

                if (waypoint.RemoveOnReached)
                    waypoints.RemoveAt(waypointIndex);
                else
                    waypointIndex++;

                if (waypointIndex >= waypoints.Count)
                    waypointIndex = 0;
            }
        }

        public void RotateToNextTarget(Vector3 targetLookPos, bool stopOnSharpAngles, float maxSharpAngle = 25.0f)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(targetLookPos - transform.position);
            lookOnLook.x = 0;
            lookOnLook.z = 0;

            if (stopOnSharpAngles)
            {
                float angle = Mathf.Abs(lookOnLook.eulerAngles.y - transform.rotation.eulerAngles.y);
                agent.isStopped =angle > maxSharpAngle && angle <= 180.0f;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, rotationSpeed * Time.deltaTime);
        }

        private void React()
        {

        }

        private void OpenDialog()
        {
            throw new System.NotImplementedException();
        }

        private void Sleep()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Check all gameobjects in hearrange to see if they are recognized
        /// </summary>
        private void Hear()
        {
            for (int i = 0; i < possibleHearedObjects.Count; i++)
            {
                bool alreadyNoticed = noticedGameObjects.Contains(possibleHearedObjects[i].gameObject);
                if(Vector3.Distance(possibleHearedObjects[i].gameObject.transform.position, transform.position) 
                    < possibleHearedObjects[i].maxDistance && possibleHearedObjects[i].isPlaying)
                { 
                    if(!alreadyNoticed)
                        noticedGameObjects.Add(possibleHearedObjects[i].gameObject);
                }
                else
                {
                    if(alreadyNoticed)
                        noticedGameObjects.Remove(possibleHearedObjects[i].gameObject);
                }
            }
        }

        /// <summary>
        /// Check all gameobjects in viewrange to see if they are in sight
        /// </summary>
        private void Recognize()
        {
            for (int i = 0; i < possibleSeenObjects.Count; i++)
            {
                Vector3 targetDir = possibleSeenObjects[i].transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                bool alreadyNoticed = noticedGameObjects.Contains(possibleSeenObjects[i]);
                if (angle < fov)
                {
                    if (!alreadyNoticed)
                    {
                        noticedGameObjects.Add(possibleSeenObjects[i]);
                    }
                }
                else
                {
                    if (alreadyNoticed)
                    {
                        noticedGameObjects.Remove(possibleSeenObjects[i]);
                    }
                }
            }
        }

        public override void Interact()
        {
            OpenDialog();
        }

        public abstract void Flee();

        /// <summary>
        /// A given object entered the hear area
        /// </summary>
        /// <param name="audioSource">Audiosource of entered gameobject</param>
        public void OnEnterHearArea(AudioSource audioSource)
        {
            possibleHearedObjects.Add(audioSource);
        }

        /// <summary>
        /// A given object left the hear area
        /// </summary>
        /// <param name="audioSource">Audiosource of the left gameobject</param>
        public void OnExitHearArea(AudioSource audioSource)
        {
            possibleHearedObjects.Remove(audioSource);
        }

        /// <summary>
        /// A given object entered the view area
        /// </summary>
        /// <param name="other">Collider of gameobject</param>
        public void OnEnterViewArea(Collider other)
        {
            possibleSeenObjects.Add(other.gameObject);
        }

        /// <summary>
        /// A given object left the view area
        /// </summary>
        /// <param name="other">Collider of gameobject</param>
        public void OnExitViewArea(Collider other)
        {
            possibleSeenObjects.Remove(other.gameObject);
        }
    }
}
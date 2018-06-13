using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RuthlessMerchant
{
    public abstract class NPC : Character
    {
        public enum SpeedType
        {
            None,
            Walk,
            Run,
            Stealth,
            Crouch,
            Jump
        }

        [Flags]
        public enum TargetState
        {
            None,
            InView,
            Lost,
            Search,
            IsThreat
        }

        private DayCycle dayCycle;
        private DialogSystem dialogSystem;

        protected List<Waypoint> waypoints;

        [HideInInspector]
        public Nullable<Waypoint> CurrentWaypoint = null;

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

        protected NavMeshAgent agent;

        private List<GameObject> possibleSeenObjects;
        private List<AudioSource> possibleHearedObjects;

        private List<GameObject> noticedGameObjects;
        private List<GameObject> noticedThreats;

        public event EventHandler OnCharacterNoticed;
        public event EventHandler OnItemNoticed;
        public event EventHandler OnHeardSomething;

        private ActionNPC currentAction;

        private float elapsedWaitTime = 0.0f;
        private Vector3 rotationTarget;
        public bool Reacting;

        private Character currentReactTarget;
        private Item currentItemTarget;
        private Vector3 currentReactPosition;
        private TargetState reactionState;
        private float elapsedLostTime = 0.0f;
        private float lostDuration = 3.0f;

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

        public float RotationSpeed
        {
            get
            {
                return rotationSpeed;
            }
        }

        public List<Waypoint> Waypoints
        {
            get
            {
                return waypoints;
            }
        }

        public ActionNPC CurrentAction
        {
            get
            {
                return currentAction;
            }
            set
            {
                if(currentAction != null)
                    currentAction.EndAction();

                currentAction = value;
            }
        }

        public override void Start()
        {
            possibleHearedObjects = new List<AudioSource>();
            possibleSeenObjects = new List<GameObject>();
            noticedGameObjects = new List<GameObject>();
            noticedThreats = new List<GameObject>();

            if(waypoints == null)
                waypoints = new List<Waypoint>();

            GameObject hearObject = transform.GetChild(0).gameObject;
            GameObject seeObject = transform.GetChild(1).gameObject;

            SphereCollider seeCollider = seeObject.GetComponent<SphereCollider>();
            seeCollider.radius = viewDistance;

            SphereCollider hearCollider = hearObject.GetComponent<SphereCollider>();
            hearCollider.radius = hearDistance;

            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;

            ChangeSpeed(SpeedType.Walk);

            //agent.updateRotation = false;
        }

        public override void Update()
        {
            base.Update();
            Recognize();
            Hear();

            if(currentAction != null)
                currentAction.Update(Time.deltaTime);

            if (currentReactTarget != null)
            { 
                if(reactionState.HasFlag(TargetState.Lost))
                {
                    elapsedLostTime += Time.deltaTime;
                    if(elapsedLostTime >= lostDuration)
                    {
                        reactionState = reactionState.Clear();
                        currentReactTarget = null;
                        elapsedLostTime = 0;
                        Debug.Log("lost target => currentReactTarget = null");
                    }
                }

                if (currentReactTarget != null)
                {
                    Debug.Log("Reaction target: " + currentReactTarget.name);
                    React(currentReactTarget, reactionState.HasFlag(TargetState.IsThreat));
                }
            }
            else if (currentItemTarget != null)
                React(currentItemTarget);

            if(!Reacting)
                ChangeSpeed(SpeedType.Walk);
        }

        public void ChangeSpeed(SpeedType speedType)
        {
            switch (speedType)
            {
                case SpeedType.None:
                    agent.speed = 0;
                    agent.angularSpeed = 0;
                    break;
                case SpeedType.Walk:
                    agent.speed = walkSpeed;
                    agent.angularSpeed = walkSpeed;
                    break;
                case SpeedType.Run:
                    agent.speed = runSpeed;
                    agent.angularSpeed = runSpeed;
                    break;
                case SpeedType.Stealth:
                    agent.speed = walkSpeed;
                    agent.angularSpeed = walkSpeed;
                    break;
                case SpeedType.Crouch:
                    agent.speed = walkSpeed;
                    agent.angularSpeed = walkSpeed;
                    break;
                case SpeedType.Jump:
                    agent.speed = runSpeed;
                    agent.angularSpeed = runSpeed;
                    break;
                default:
                    break;
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

        private void FindReactionTarget(GameObject gameObject)
        {
            Character character = gameObject.GetComponent<Character>();
            if (character != null)
            {
                if (IsThreat(gameObject))
                {
                    if (currentReactTarget != null)
                    {
                        if (Vector3.Distance(gameObject.transform.position, transform.position) <
                            Vector3.Distance(currentReactTarget.transform.position, transform.position))
                        {
                            currentReactTarget = character;
                        }
                    }
                    else
                    {
                        currentReactTarget = character;
                    }

                    //character is only a potential threat if one of these characters isn't a civilian (prevents a civ vs civ fight where both civs flee)
                    if (!(this is Civilian && character is Civilian))
                        reactionState = reactionState.SetFlag(TargetState.IsThreat);
                }
                else
                {
                    if(!reactionState.HasFlag(TargetState.IsThreat))
                    {
                        if (Vector3.Distance(gameObject.transform.position, transform.position) <
                            Vector3.Distance(currentReactTarget.transform.position, transform.position))
                        {
                            currentReactTarget = character;
                        }
                    }
                }
            }
            else
            {
                Item item = gameObject.GetComponent<Item>();
                if(item != null)
                {
                    //TODO: React to item?
                    currentItemTarget = item;
                }
            }
        }

        public abstract void React(Character character, bool isThreat);
        public abstract void React(Item item);

        public void AddNewWaypoint(Waypoint waypoint, bool clearWaypoints = false)
        {
            if (clearWaypoints)
                waypoints.Clear();

            waypoints.Add(waypoint);
        }
        
        protected bool IsThreat(GameObject gameObject)
        {
            Character character = gameObject.GetComponent<Character>();
            if (character != null && faction != character.Faction)
            {
                if (character.IsPlayer)
                {
                    //TODO check faction standings
                    return false;
                }
                else
                    return true;
            }

            return false;
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
                    if (!alreadyNoticed)
                    {
                        noticedGameObjects.Add(possibleHearedObjects[i].gameObject);
                        FindReactionTarget(possibleHearedObjects[i].gameObject);
                    }
                }
                else
                {
                    if (alreadyNoticed)
                    {
                        noticedGameObjects.Remove(possibleHearedObjects[i].gameObject);
                        if (currentReactTarget.gameObject == possibleHearedObjects[i].gameObject)
                            currentReactTarget = null;
                    }
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
                if(possibleSeenObjects[i] == null)
                {
                    possibleSeenObjects.RemoveAt(i);
                    i--;
                    continue;
                }

                Vector3 targetDir = possibleSeenObjects[i].transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                
                bool alreadyNoticed = noticedGameObjects.Contains(possibleSeenObjects[i]);
                Vector3 direction = transform.position - possibleSeenObjects[i].transform.position;
                direction.Normalize();
                //Debug.Log("Ray: " + Physics.Raycast(transform.position, direction, viewDistance, 0).ToString());
                //TODO FIX RAYCAST
                if (angle < fov) //&& Physics.Raycast(transform.position, direction, viewDistance, 0))
                {
                    if (!alreadyNoticed)
                    {
                        noticedGameObjects.Add(possibleSeenObjects[i]);
                        FindReactionTarget(possibleSeenObjects[i]);
                        if(currentReactTarget != null && currentReactTarget.gameObject == possibleSeenObjects[i])
                        {
                            reactionState = reactionState.SetFlag(TargetState.InView);
                            reactionState = reactionState.RemoveFlag(TargetState.Lost);
                            elapsedLostTime = 0.0f;
                        }
                    }
                }
                else
                {
                    if (alreadyNoticed)
                    {
                        noticedGameObjects.Remove(possibleSeenObjects[i]);
                        if (currentReactTarget != null && currentReactTarget.gameObject == possibleSeenObjects[i])
                        {
                            elapsedLostTime = 0.0f;
                            reactionState = reactionState.SetFlag(TargetState.Lost);
                            reactionState = reactionState.RemoveFlag(TargetState.InView);
                        }
                    }
                }
            }
        }

        public void SetRandomPath(string[] paths, float waitTime)
        {
            string path = paths[UnityEngine.Random.Range(0, paths.Length - 1)];
            if (path != null)
            {
                GameObject parentPath = GameObject.Find(path);
                List<Waypoint> waypoints = new List<Waypoint>();
                foreach (Transform child in parentPath.transform)
                {
                    if (child.CompareTag(path))
                        waypoints.Add(new Waypoint(child, true, waitTime));
                }

                AddPath(waypoints);
            }
        }

        public Waypoint[] GetRandomPath(string[] paths, bool removeOnReached, float waitTime)
        {
            if (paths != null && paths.Length > 0)
            {
                string path = paths[UnityEngine.Random.Range(0, paths.Length - 1)];
                if (path != null)
                {
                    GameObject parentPath = GameObject.Find(path);
                    List<Waypoint> waypoints = new List<Waypoint>();
                    foreach (Transform child in parentPath.transform)
                    {
                        if (child.CompareTag(path))
                            waypoints.Add(new Waypoint(child, removeOnReached, waitTime));
                    }

                    return waypoints.ToArray();
                }
            }
            return null;
        }

        public void AddPath(IEnumerable<Waypoint> path)
        {
            if (waypoints == null)
                waypoints = new List<Waypoint>();

            waypoints.AddRange(path);
        }

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
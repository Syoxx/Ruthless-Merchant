//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

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
            Jump,
            Flee
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

        private Vector3 rotationTarget;
        public bool Reacting;

        private Character currentReactTarget;
        private Item currentItemTarget;
        private Vector3 currentReactPosition;
        private TargetState reactionState;
        private SpeedType currentSpeedType = SpeedType.None;
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

        /// <summary>
        /// Rotation speed
        /// </summary>
        public float RotationSpeed
        {
            get
            {
                return rotationSpeed;
            }
        }

        /// <summary>
        /// Current path
        /// </summary>
        public List<Waypoint> Waypoints
        {
            get
            {
                return waypoints;
            }
        }

        /// <summary>
        /// Current action
        /// </summary>
        public ActionNPC CurrentAction
        {
            get
            {
                return currentAction;
            }
        }

        /// <summary>
        /// Current reaction target
        /// </summary>
        public Character CurrentReactTarget
        {
            get
            {
                return currentReactTarget;
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
                        reactionState = TargetState.None;
                        currentReactTarget = null;
                        elapsedLostTime = 0;
                    }
                }

                if (currentReactTarget != null)
                {
                    React(currentReactTarget, reactionState.HasFlag(TargetState.IsThreat));
                }
            }
            else if (currentItemTarget != null)
                React(currentItemTarget);

            if(!Reacting)
                ChangeSpeed(SpeedType.Walk);
        }

        /// <summary>
        /// Checks whether a given gameobject is a threat or not
        /// </summary>
        /// <param name="gameObject">Gameobject to check</param>
        /// <returns>Returns true when the gameobject is a potential threat</returns>
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
                        if (OnHeardSomething != null)
                            OnHeardSomething.Invoke(this, null);
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

                if (angle < fov && !IsObjectBehindObstacle(possibleSeenObjects[i]))
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

                        //TODO: check components and add event args!
                        if (OnCharacterNoticed != null)
                            OnCharacterNoticed.Invoke(this, null);

                        if (OnItemNoticed != null)
                            OnItemNoticed.Invoke(this, null);
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

        /// <summary>
        /// Checks whether a given object can be seen by a NPC or not
        /// </summary>
        /// <param name="gameObject">Object to check</param>
        /// <returns>Returns true if the object is covered by an obstacle</returns>
        private bool IsObjectBehindObstacle(GameObject gameObject)
        {
            Vector3 direction = gameObject.transform.position - transform.position;
            direction.Normalize();

            RaycastHit hitInfo;
            bool hit = Physics.Raycast(transform.position, direction, out hitInfo, viewDistance);
            if (hit)
                hit = hitInfo.collider.gameObject == gameObject;

            return !hit;
        }

        /// <summary>
        /// Checks if the NPC has to react to the given gameobject
        /// </summary>
        /// <param name="gameObject">Gameobject to check</param>
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
                    if (!reactionState.HasFlag(TargetState.IsThreat))
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
                if (item != null)
                {
                    //TODO: React to item?
                    currentItemTarget = item;
                }
            }
        }

        #region Publics
        /// <summary>
        /// React methode to react on an character
        /// </summary>
        /// <param name="character">Character to react to</param>
        /// <param name="isThreat">Indicates whether the character is a threat or not</param>
        public abstract void React(Character character, bool isThreat);

        /// <summary>
        /// React methode to react to an item
        /// </summary>
        /// <param name="item">Item to react to</param>
        public abstract void React(Item item);

        /// <summary>
        /// Sets the current action
        /// </summary>
        /// <param name="action">Action to set</param>
        /// <param name="other">Gameobject which might be useful for the action start</param>
        public void SetCurrentAction(ActionNPC action, GameObject other)
        {
            if (currentAction != null)
                currentAction.EndAction();

            currentAction = action;
            currentAction.StartAction(this, other);
        }

        /// <summary>
        /// Rotate towards a given position
        /// </summary>
        /// <param name="targetLookPos">Position to look at</param>
        /// <param name="stopOnSharpAngles">Indicates whether the character should stop on sharp rotations or not</param>
        /// <param name="maxSharpAngle">Indicates when an angle is considered as sharp</param>
        public void RotateToNextTarget(Vector3 targetLookPos, bool stopOnSharpAngles, float maxSharpAngle = 25.0f)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(targetLookPos - transform.position);
            lookOnLook.x = 0;
            lookOnLook.z = 0;

            if (stopOnSharpAngles)
            {
                float angle = Mathf.Abs(lookOnLook.eulerAngles.y - transform.rotation.eulerAngles.y);
                agent.isStopped = angle > maxSharpAngle && angle <= 180.0f;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Changes the move speed based on the speedtype
        /// </summary>
        /// <param name="speedType">Speedtype</param>
        public void ChangeSpeed(SpeedType speedType)
        {
            if (currentSpeedType == speedType)
                return;

            currentSpeedType = speedType;
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
                case SpeedType.Flee:
                    agent.speed = runSpeed * 1.5f;
                    agent.angularSpeed = runSpeed * 1.5f;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds a new waypoint to the current path
        /// </summary>
        /// <param name="waypoint">Waypoint which will be added to the end of the path</param>
        /// <param name="clearWaypoints">Indicates whether the current path should be removed or not</param>
        public void AddNewWaypoint(Waypoint waypoint, bool clearWaypoints = false)
        {
            if (clearWaypoints)
                waypoints.Clear();

            waypoints.Add(waypoint);
        }

        /// <summary>
        /// Sets a random path which is choosen from an array of path names
        /// </summary>
        /// <param name="paths">Array of path names</param>
        /// <param name="waitTime">Wait time on a waypoint</param>
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

        /// <summary>
        /// Returns an array of waypoints which are choosen from an array of path names
        /// </summary>
        /// <param name="paths">Array of path names</param>
        /// <param name="removeOnReached">Indicates whether the waypoint should be removed or not when the waypoint was reached</param>
        /// <param name="waitTime">Wait time on a waypoint</param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a given path to the current path
        /// </summary>
        /// <param name="path">Array of waypoints</param>
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
        #endregion
    }
}
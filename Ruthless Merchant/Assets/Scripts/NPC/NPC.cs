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
        public static int MaxNPCCountPerFaction = 62;
        public static Dictionary<Faction, int> NPCCount = new Dictionary<Faction, int>()
        {
            { Faction.Freidenker, 0 },
            { Faction.Imperialisten,0 },
            { Faction.Monster, 0 }
        };

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

        private DialogSystem dialogSystem;

        protected List<Waypoint> waypoints;

        [HideInInspector]
        public Nullable<Waypoint> CurrentWaypoint = null;

        [SerializeField, Range(0.1f, 100.0f), Tooltip("Roation Speed")]
        protected float rotationSpeed = 8.0f;

        [Header("NPC Recogniction Settings")]
        [SerializeField, Range(25, 90), Tooltip("Field of view")]
        protected int fov = 45;

        [SerializeField, Range(1, 100), Tooltip("Max. view distance")]
        protected int viewDistance = 10;

        [SerializeField, Range(1, 100), Tooltip("Max. hear distance of npcs. Is used by child gameoject with hear script")]
        protected int hearDistance = 5;

        [Header("NPC Settings")]

        [SerializeField, Range(0, 100), Tooltip("Capturing value per second")]
        protected int capValuePerSecond = 1;

        public int CapValuePerSecond
        {
            get
            {
                return capValuePerSecond;
            }
        }

        protected NavMeshAgent agent;
        protected int laneSelectionIndex = 0;

        private List<GameObject> possibleSeenObjects;

        private List<GameObject> possibleHearedObjects;

        public event EventHandler OnCharacterNoticed;
        public event EventHandler OnItemNoticed;

        private ActionNPC currentAction;

        private Vector3 rotationTarget;

        [HideInInspector]
        public bool Reacting;

        private Character currentReactTarget;
        private Item currentItemTarget;
        private Vector3 currentReactPosition;
        private TargetState reactionState;
        private SpeedType currentSpeedType = SpeedType.None;
        private float elapsedLostTime = 0.0f;
        private float lostDuration = 3.0f;

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
            possibleSeenObjects = new List<GameObject>();
            possibleHearedObjects = new List<GameObject>();

            if (waypoints == null)
                waypoints = new List<Waypoint>();

            Transform seeObj = transform.Find("SeeCollider");
            if (seeObj != null)
            {
                SphereCollider seeCollider = seeObj.GetComponent<SphereCollider>();
                seeCollider.radius = viewDistance;
            }

            Transform hearObj = transform.Find("HearCollider");
            if(hearObj != null)
            {
                SphereCollider hearCollider = hearObj.GetComponent<SphereCollider>();
                hearCollider.radius = hearDistance;
            }

            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            if (agent.stoppingDistance == 0)
                agent.stoppingDistance = 1;

            ChangeSpeed(SpeedType.Walk);

            NPCCount[faction]++;
            base.Start();
        }

        public override void Update()
        {
            base.Update();
            if (!isDying)
            {
                Recognize();

                if (currentAction != null)
                    currentAction.Update(Time.deltaTime);

                CheckReactionState();

                if (!Reacting)
                {
                    ChangeSpeed(SpeedType.Walk);
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.isStopped = true;
                }
            }
            else
            {
                SetCurrentAction(null, null, true, true);
            }
        }

        public override void DestroyInteractiveObject(float delay = 0)
        {
            NPCCount[faction]--;
            base.DestroyInteractiveObject(delay);
        }

        private void CheckReactionState()
        {
            if (currentReactTarget != null)
            {
                //Check Target Health
                if (CurrentReactTarget.HealthSystem == null || CurrentReactTarget.HealthSystem.Health <= 0)
                {
                    reactionState = TargetState.None;
                    currentReactTarget = null;
                    elapsedLostTime = 0;
                }

                //Check lost targets
                if (reactionState.HasFlag(TargetState.Lost))
                {
                    elapsedLostTime += Time.deltaTime;
                    if (elapsedLostTime >= lostDuration)
                    {
                        reactionState = TargetState.None;
                        currentReactTarget = null;
                        elapsedLostTime = 0;
                    }
                }
            }

            //Execute Reaction
            if (currentReactTarget != null)
            {
                React(currentReactTarget, reactionState.HasFlag(TargetState.IsThreat));
            }
            else if (currentItemTarget != null)
                React(currentItemTarget);
            else if (!(currentAction is ActionIdle))
                SetCurrentAction(new ActionIdle(), null);
        }

        /// <summary>
        /// Checks whether a given gameobject is a threat or not
        /// </summary>
        /// <param name="gameObject">Gameobject to check</param>
        /// <returns>Returns true when the gameobject is a potential threat</returns>
        protected bool IsThreat(GameObject gameObject)
        {
            Character character = gameObject.GetComponent<Character>();
            if (character != null && faction != character.Faction && character.HealthSystem != null && character.HealthSystem.Health > 0)
            {
                if (character.IsPlayer && faction != Faction.Monster)
                {
                    return false;
                }
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check all gameobjects in viewrange to see if they are in sight
        /// </summary>
        private void Recognize()
        {
            for (int i = 0; i < possibleSeenObjects.Count; i++)
            {
                if (possibleSeenObjects[i] == null)
                {
                    possibleSeenObjects.RemoveAt(i);
                    i--;
                    continue;
                }

                Vector3 targetDir = possibleSeenObjects[i].transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (angle < fov && !IsObjectBehindObstacle(possibleSeenObjects[i]))
                {
                    if (currentReactTarget != null && currentReactTarget == possibleSeenObjects[i])
                    {
                        elapsedLostTime = 0.0f;
                        reactionState = reactionState.SetFlag(TargetState.Lost);
                        reactionState = reactionState.RemoveFlag(TargetState.InView);
                    }
                    else
                    {
                        FindReactionTarget(possibleSeenObjects[i]);
                        if (currentReactTarget != null && currentReactTarget.gameObject == possibleSeenObjects[i])
                        {
                            reactionState = reactionState.SetFlag(TargetState.InView);
                            reactionState = reactionState.RemoveFlag(TargetState.Lost);
                            elapsedLostTime = 0.0f;

                            if (OnCharacterNoticed != null)
                                OnCharacterNoticed.Invoke(this, null);
                        }
                    }
                }
            }

            if(currentReactTarget == null)
            {
                for (int i = 0; i < possibleHearedObjects.Count; i++)
                {
                    FindReactionTarget(possibleHearedObjects[i]);
                    if(currentReactTarget != null)
                    {
                        if (OnCharacterNoticed != null)
                            OnCharacterNoticed.Invoke(this, null);

                        if (reactionState.HasFlag(TargetState.IsThreat))
                            break;
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
            /*  Vector3 direction = gameObject.transform.position - transform.position;
              direction.Normalize();

              RaycastHit hitInfo;
              bool hit = Physics.Raycast(transform.position, direction, out hitInfo, viewDistance);
              if (hit)
                  hit = hitInfo.collider.gameObject == gameObject;

              return !hit;*/
            return false;
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
                    if (currentReactTarget != null && reactionState.HasFlag(TargetState.IsThreat))
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
        public void SetCurrentAction(ActionNPC action, GameObject other, bool force = false, bool executeEnd = true)
        {
            if ((!isDying && (force || currentAction == null || currentAction.Priority <= action.Priority)) || (isDying && action == null))
            {
                if (currentAction != null)
                    currentAction.EndAction(executeEnd);

                currentAction = action;
                if(currentAction != null)
                    currentAction.StartAction(this, other);
            }
        }


        public void ResetTarget()
        {
            currentReactTarget = null;
            reactionState = TargetState.None;
            Reacting = false;
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
                    break;
                case SpeedType.Walk:
                    agent.speed = walkSpeed;
                    break;
                case SpeedType.Run:
                    agent.speed = runSpeed;
                    break;
                case SpeedType.Stealth:
                    agent.speed = walkSpeed;
                    break;
                case SpeedType.Crouch:
                    agent.speed = walkSpeed;
                    break;
                case SpeedType.Jump:
                    agent.speed = runSpeed;
                    break;
                case SpeedType.Flee:
                    agent.speed = runSpeed * 1.5f;
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
            if (waypoints == null)
                waypoints = new List<Waypoint>();

            if (clearWaypoints)
                waypoints.Clear();

            waypoints.Add(waypoint);
        }

        /// <summary>
        /// Sets a random path which is choosen from an array of path names
        /// </summary>
        /// <param name="paths">Array of path names</param>
        /// <param name="waitTime">Wait time on a waypoint</param>
        /// <returns>Returns true on success</returns>
        public bool SetRandomPath(string[] paths, float waitTime)
        {
            if (paths != null && paths.Length > 0)
            {
                string path = paths[UnityEngine.Random.Range(0, paths.Length)];
                return SetPath(path, waitTime) != null;
            }
            return false;
        }

        /// <summary>
        /// Sets a given path
        /// </summary>
        /// <param name="path">Name of path</param>
        /// <param name="waitTime">Wait time on each waypoint</param>
        /// <returns>Returns a list of waypoints</returns>
        public IEnumerable<Waypoint> SetPath(string path, float waitTime, bool removeOnWaypointReached = true)
        {
            if (path != null)
            {
                GameObject parentPath = GameObject.Find(path);
                List<Waypoint> waypoints = new List<Waypoint>();
                foreach (Transform child in parentPath.transform)
                {
                    if (child.CompareTag(path))
                        waypoints.Add(new Waypoint(child, removeOnWaypointReached, waitTime));
                }

                AddWaypoints(waypoints);

                return waypoints;
            }

            return null;
        }

        /// <summary>
        /// Adds a new waypoint
        /// </summary>
        /// <param name="path">Next capture trigger</param>
        /// <param name="waitTime">Wait time on waypoint</param>
        /// <returns>Returns a waypoint</returns>
        public Waypoint SetPath(CaptureTrigger path, float waitTime, bool removeOnWaypointReached = true, int laneSelectionIndex = 0)
        {
            if (path != null)
            {
                this.laneSelectionIndex = laneSelectionIndex;
                Waypoint waypoint = new Waypoint(path.Target, removeOnWaypointReached, waitTime);
                AddNewWaypoint(waypoint, true);
                return waypoint;
            }

            return default(Waypoint);
        }

        /// <summary>
        /// Returns an array of waypoints which are choosen from an array of path names
        /// </summary>
        /// <param name="paths">Array of path names</param>
        /// <param name="removeOnReached">Indicates whether the waypoint should be removed or not when the waypoint was reached</param>
        /// <param name="waitTime">Wait time on a waypoint</param>
        /// <returns>Returns a list of waypoints</returns>
        public IEnumerable<Waypoint> GetRandomPath(string[] paths, bool removeOnReached, float waitTime)
        {
            if (paths != null && paths.Length > 0)
            {
                string path = paths[UnityEngine.Random.Range(0, paths.Length)];
                return SetPath(path, waitTime, removeOnReached);
            }
            return null;
        }

        /// <summary>
        /// Adds a given path to the current path
        /// </summary>
        /// <param name="path">Array of waypoints</param>
        public void AddWaypoints(IEnumerable<Waypoint> path)
        {
            if (waypoints == null)
                waypoints = new List<Waypoint>();

            waypoints.AddRange(path);
        }

        public virtual void ChangeFaction(Faction newFaction)
        {
            faction = newFaction;
        }

        /// <summary>
        /// A given object entered the view area
        /// </summary>
        /// <param name="other">Collider of gameobject</param>
        public void OnEnterViewArea(Collider other)
        {
            if (possibleSeenObjects == null)
                possibleSeenObjects = new List<GameObject>();

            possibleSeenObjects.Add(other.gameObject);
        }

        /// <summary>
        /// A given object left the view area
        /// </summary>
        /// <param name="other">Collider of gameobject</param>
        public void OnExitViewArea(Collider other)
        {
            if (possibleSeenObjects == null)
                possibleSeenObjects = new List<GameObject>();

            possibleSeenObjects.Remove(other.gameObject);
        }

        /// <summary>
        /// A given object entered the hear area
        /// </summary>
        /// <param name="other">Collider of gameobject</param>
        public void OnEnterHearArea(Collider other)
        {
            if (possibleHearedObjects == null)
                possibleHearedObjects = new List<GameObject>();

            possibleHearedObjects.Add(other.gameObject);
        }

        /// <summary>
        /// A given object left the hear area
        /// </summary>
        /// <param name="other">Collider of gameobject</param>
        public void OnExitHearArea(Collider other)
        {
            if (possibleHearedObjects == null)
                possibleHearedObjects = new List<GameObject>();

            possibleHearedObjects.Remove(other.gameObject);
        }
        #endregion
    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SCPNewView.Management;
using SCPNewView.Utils;
using UnityEngine;
using Pathfinding;
using System;
using SCPNewView.Saving;

namespace SCPNewView.Entities.SCP049 {
    [RequireComponent(typeof(Seeker), typeof(AIPath), typeof(AIDestinationSetter))]
    public class SCP049 : MonoBehaviour, IDataPersisting {
        public bool SeesAnyTarget => _validTargetList.Count != 0;
        public List<Transform> Targets => _validTargetList;
        public (Seeker Seeker, AIPath Path, AIDestinationSetter Dest) PathfindingData => _pathfindingData;
        public float Speed { get => _pathfindingData.path.maxSpeed; set => _pathfindingData.path.maxSpeed = value; }

        [SerializeField] Vector2 debug_pathfindingLocation;

        private IState _currentState;

        public IState ChaseState { get; private set; }
        public IState IdleState { get; private set; }

        private List<Tag> _targetTags;
        private List<Transform> _cachedTargetList;
        private List<Transform> _validTargetList;
        private Transform _pathfindTransform;
        private (Seeker seeker, AIPath path, AIDestinationSetter dest) _pathfindingData;

        private void Awake() {
            _targetTags = ReferenceManager.Current.FriendlyEntityTags;
            _validTargetList = new List<Transform>();
            _pathfindingData = (GetComponent<Seeker>(), GetComponent<AIPath>(), GetComponent<AIDestinationSetter>());
            _pathfindingData.path.maxAcceleration = 1000f;
            _pathfindTransform = new GameObject("SCP049 Pathfind Target").transform;

            ChaseState = new ChaseState(this);
            IdleState = new IdleState(this);

            SetState(IdleState);

            EventSystem.NewEntitySpawned += UpdateEntityListCache;
            UpdateEntityListCache();
        }
        private void Start() {
            transform.position = DataPersistenceManager.Current.Scp049Data.Position;
            StartCoroutine(TargetDetection());
        }
        private void Update() {
            _currentState?.OnUpdateTick();
        }

        public void SetState(IState newState) {
            if (_currentState == newState) { return; }
            _currentState?.OnExitState();
            _currentState = newState;
            _currentState?.OnEnterState();
        }
        public void PathfindToLocation(Vector2 location) {
            _pathfindTransform.position = location;
            _pathfindingData.dest.target = _pathfindTransform;
        }
        private IEnumerator TargetDetection() {
            while (true) {
                _validTargetList.Clear();
                foreach (Transform toCheck in _cachedTargetList) {
                    Vector2 dirToTarget = toCheck.position - transform.position;
                    RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirToTarget, Vector2.Distance(transform.position, toCheck.position));

                    #if UNITY_EDITOR
                    if (rayHit.collider == null) continue; // I can't see a situation this statement ever returns true in prod, but theres a chance it happens in debug.
                    #endif
                    TagList colliderRayHit = rayHit.collider.GetComponent<TagList>();
                    if (!colliderRayHit) continue; // Walls shouldn't ever have a TagList component, so if this statement returns true its likely a wall (or a non entity)
                    if (!colliderRayHit.HasAnyTag(_targetTags.ToArray())) continue;

                    _validTargetList.Add(toCheck);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        private void OnDestroy() {
            EventSystem.NewEntitySpawned -= UpdateEntityListCache;
        }
        private void UpdateEntityListCache() {
            List<TagList> targets = FindObjectsOfType<TagList>().Where((tl) => tl.HasAnyTag(_targetTags.ToArray())).ToList();
            _cachedTargetList = targets.Select(x => x.transform).ToList();
        }
        [ContextMenu("Pathfind To Location")]
        private void DebugPathfind() {
            PathfindToLocation(debug_pathfindingLocation);
        }
        public void OnGameSave() {
            DataPersistenceManager.Current.Scp049Data.Position = transform.position;
        }
    }
    public interface IState {
        void OnEnterState();
        void OnUpdateTick();
        void OnExitState();
    }
    public class ChaseState : IState {
        private SCP049 _ctx;
        private Transform _target;

        public ChaseState(SCP049 ctx) {
            _ctx = ctx;
        }

        public void OnEnterState() {
            // TODO : Play 049 Chase VO
            Transform[] targets = _ctx.Targets.ToArray();
            targets = targets.OrderBy((x) => Vector2.Distance(_ctx.transform.position, x.position)).ToArray();
            _target = targets[0];
        }

        public void OnExitState() {
            _target = null;
        }

        public void OnUpdateTick() {
            _ctx.PathfindToLocation(_target.position);
            if (!_ctx.SeesAnyTarget) {
                _ctx.SetState(_ctx.IdleState);
            }
        }
    }
    public class IdleState : IState {
        private SCP049 _ctx;
        public IdleState(SCP049 ctx) => _ctx = ctx;
        public void OnEnterState() {
            return;
        }

        public void OnExitState() {
            return;
        }
        
        public void OnUpdateTick() {
            if (_ctx.SeesAnyTarget) {
                _ctx.SetState(_ctx.ChaseState);
            }
        }
    }
}
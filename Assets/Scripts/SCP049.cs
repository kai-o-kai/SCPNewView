using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SCPNewView.Management;
using SCPNewView.Utils;
using UnityEngine;

namespace SCPNewView.Entities.SCP049 {
    public class SCP049 : MonoBehaviour {
        public bool SeesAnyTarget => _validTargetList.Count != 0;
        public List<Transform> Targets => _validTargetList;

        private List<Tag> _targetTags;
        private IState _currentState;
        private List<Transform> _cachedTargetList;
        private List<Transform> _validTargetList;

        private void Awake() {
            _targetTags = ReferenceManager.Current.FriendlyEntityTags;
            _validTargetList = new List<Transform>();
            EventSystem.NewEntitySpawned += UpdateEntityListCache;
            UpdateEntityListCache();
        }
        private void Start() {
            StartCoroutine(TargetDetection());
        }
        private void Update() {
            _currentState?.OnUpdateTick();
        }

        public void SwitchStates(IState newState) {
            if (_currentState == newState) { return; }
            _currentState?.OnExitState();
            _currentState = newState;
            _currentState?.OnEnterState();
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
    }
    public interface IState {
        void OnEnterState();
        void OnUpdateTick();
        void OnExitState();
    }
}
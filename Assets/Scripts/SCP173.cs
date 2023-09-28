using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SCPNewView.Management;
using UnityEngine;
using SCPNewView.Utils;
using Pathfinding;

namespace SCPNewView.Entities.SCP173 {
    [RequireComponent(typeof(AIDestinationSetter), typeof(AIDestinationSetter))]
    public class SCP173 : MonoBehaviour, ILightable, ILookable {
        public Dictionary<Light, bool> IsLitBy { get; } = new Dictionary<Light, bool>();
        public Dictionary<Looker, bool> IsLookedAtBy { get; } = new Dictionary<Looker, bool>();

        private Transform _closestTarget => _targets.Count != 0 ? _targets[0] : null;
        private bool _hasTargets => _targets.Count != 0;

        private bool _isLit => IsLitBy.Any((x) => x.Value == true);
        private bool _isLooked => IsLookedAtBy.Any((x) => x.Value == true);

        private List<Transform> _cachedPossibleTargetList = new();
        private List<Transform> _targets = new();
        private List<Tag> _targetTags;

        private AIPath _path;
        private AIDestinationSetter _dest;
        private Transform _pathfindingObj;

        [SerializeField] private float _speed;

        private void Awake() {
            ILightable.AddLightableObject(transform);
            ILookable.AddLookable(transform);
            _path = GetComponent<AIPath>();
            _dest = GetComponent<AIDestinationSetter>();
        }
        private void Start() {
            _pathfindingObj = new GameObject("SCP173 Pathfind Obj").transform;
            _targetTags = ReferenceManager.Current.FriendlyEntityTags;
            Light.LightListChanged += UpdateLightsDic;
            UpdateTargetsListCache();
            EventSystem.NewEntitySpawned += UpdateTargetsListCache;
            StartCoroutine(TargetDetecction());
        }
        private void Update() {
            if (!_isLit || !_isLooked) {
                _path.maxSpeed = _speed;
                _path.rotationSpeed = Mathf.Infinity;
            } else if (_isLit && _isLooked) {
                _path.maxSpeed = 0f;
                _path.rotationSpeed = 0f;
            }
            if (_hasTargets) {
                PathfindToLocation(_closestTarget.position);
            } else {
                _dest.target = null;
            }
        }
        private void OnDestroy() {
            ILightable.RemoveLightableObject(transform);
            ILookable.RemoveLookable(transform);
            Light.LightListChanged -= UpdateLightsDic;
            EventSystem.NewEntitySpawned -= UpdateTargetsListCache;
        }
        private void UpdateLightsDic() {
            Light[] lights = Light.Lights;
            // TODO : This function is hella expensive for no reason. Optimise.
            foreach (var toCheckCorresponding in lights) {
                if (!IsLitBy.ContainsKey(toCheckCorresponding)) {
                    IsLitBy.Add(toCheckCorresponding, false);
                }
            }
            foreach (var toCheckCorresponding in IsLitBy) {
                if (!lights.Contains(toCheckCorresponding.Key)) {
                    IsLitBy.Remove(toCheckCorresponding.Key);
                }
            }
        }
        private void UpdateTargetsListCache() {
            List<TagList> targets = FindObjectsOfType<TagList>()?.Where((tl) => tl.HasAnyTag(_targetTags))?.ToList();
            _cachedPossibleTargetList = targets.Select(x => x.transform).ToList();
            _cachedPossibleTargetList = _cachedPossibleTargetList.OrderBy(x => Vector2.Distance(transform.position, x.position)).ToList();
        }
        private IEnumerator TargetDetecction() {
            while (true) {
                _targets.Clear();
                foreach (Transform toCheck in _cachedPossibleTargetList) {
                    Vector2 dirToTarget = toCheck.position - transform.position;
                    RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirToTarget, Vector2.Distance(transform.position, toCheck.position));

#if UNITY_EDITOR
                    if (rayHit.collider == null) continue; // I can't see a situation this statement ever returns true in prod, but theres a chance it happens in debug.
#endif

                    if (!rayHit.collider.TryGetComponent<TagList>(out var colliderRayHit)) {
                        continue;
                    } else {
                        if (!colliderRayHit.HasAnyTag(_targetTags.ToArray())) continue;
                    }
                    _targets.Add(toCheck);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        private void PathfindToLocation(Vector2 loc) {
            _pathfindingObj.position = loc;
            _dest.target = _pathfindingObj;
        }
    }
}
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SCPNewView.Management;
using UnityEngine;
using SCPNewView.Utils;

namespace SCPNewView.Entities.SCP173 {
    public class SCP173 : MonoBehaviour, ILightable, ILookable {
        public Dictionary<Light, bool> IsLitBy { get; } = new Dictionary<Light, bool>();
        public Dictionary<Looker, bool> IsLookedAtBy { get; } = new Dictionary<Looker, bool>();

        private List<Transform> _cachedPossibleTargetList = new();
        private List<Transform> _targets = new();
        private List<Tag> _targetTags;

        private void Awake() {
            ILightable.AddLightableObject(transform);
            ILookable.AddLookable(transform);
        }
        private void Start() {
            _targetTags = ReferenceManager.Current.FriendlyEntityTags;
            Light.LightListChanged += UpdateLightsDic;
            EventSystem.NewEntitySpawned += UpdateTargetsListCache;

            UpdateTargetsListCache();
        }
        private void Update() {
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
            _cachedPossibleTargetList.Select(x => x.transform).ToList();
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
    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using SCPNewView.Utils;
using SCPNewView.Management;

namespace SCPNewView {
    public class SCP096 : MonoBehaviour, ILightable, ILookable {
        public Dictionary<Light, bool> IsLitBy { get; } = new Dictionary<Light, bool>();
        public Dictionary<Looker, bool> IsLookedAtBy { get; } = new Dictionary<Looker, bool>();

        private bool _isLit => IsLitBy.Any((x) => x.Value == true);
        private bool _isLooked => IsLookedAtBy.Any((x) => x.Value == true);

        public bool LookableThroughScrambles { get; } = false;

        private bool _isActive;

        private List<Transform> _cachedPossibleTargetList = new();
        private List<Transform> _targets = new();
        private List<Tag> _targetTags;

        private AIPath _path;
        private AIDestinationSetter _dest;
        private Transform _pathfindingObj;

        private void Awake() {
            ILightable.AddLightableObject(transform);
            ILookable.AddLookable(transform);
            _path = GetComponent<AIPath>();
            _dest = GetComponent<AIDestinationSetter>();
        }
        private void Start() {
            _pathfindingObj = new GameObject("SCP096 Pathfind Obj").transform;
            _targetTags = ReferenceManager.Current.FriendlyEntityTags;
            Light.LightListChanged += UpdateLightsDic;
        }
        private void Update() {
            HandleLookDetection();
        }
        private void OnDestroy() {
            ILightable.RemoveLightableObject(transform);
            ILookable.RemoveLookable(transform);
            Light.LightListChanged -= UpdateLightsDic;
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
        private void PathfindToLocation(Vector2 loc) {
            _pathfindingObj.position = loc;
            _dest.target = _pathfindingObj;
        }
        private void HandleLookDetection() {
            if (!_isLit) return;
            if (!_isLooked) return;

                        
        }
    }
}
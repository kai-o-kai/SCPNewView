using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SCPNewView.Management;
using SCPNewView.Utils;
using SCPNewView;
using UnityEngine;

namespace SCPNewView.Entities.SCP049 {
    public class SCP049 : MonoBehaviour {
        private List<Tag> _targetTags;
        private IState _currentState;
        private List<Transform> _targets;

        private void Awake() {
            _targetTags = ReferenceManager.Current.FriendlyEntityTags;
            EventSystem.NewEntitySpawned += CacheEntityList;
            CacheEntityList();
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

            }
        }
        private void OnDestroy() {
            EventSystem.NewEntitySpawned -= CacheEntityList;
        }
        private void CacheEntityList() {
            List<TagList> targets = FindObjectsOfType<TagList>().Where((tl) => tl.HasAnyTag(_targetTags.ToArray())).ToList();
            _targets = targets.Select(x => x.transform).ToList();
        }
    }
    public interface IState {
        void OnEnterState();
        void OnUpdateTick();
        void OnExitState();
    }
}
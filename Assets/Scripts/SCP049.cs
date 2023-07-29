using UnityEngine;

namespace SCPNewView.Entities.SCP049 {
    public class SCP049 : MonoBehaviour {
        private IState _currentState;

        private void Update() {
            _currentState?.OnUpdateTick();
        }

        public void SwitchStates(IState newState) {
            if (_currentState == newState) { return; }
            _currentState?.OnExitState();
            _currentState = newState;
            _currentState?.OnEnterState();
        }
    }
    public interface IState {
        void OnEnterState();
        void OnUpdateTick();
        void OnExitState();
    }
}
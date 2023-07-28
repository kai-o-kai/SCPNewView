using UnityEngine;
using SCPNewView.Saving;
using System.Collections.Generic;
using SCPNewView.Environment;
using System;

namespace SCPNewView.Management {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance => s_instance;
        private static GameManager s_instance;
           
        public List<IInteractableEnvironment> InteractableEnvironmentPieces { get {
                EnvironmentListChanged?.Invoke();
                return _interactableEnvironmentPieces;
        }}
        private List<IInteractableEnvironment> _interactableEnvironmentPieces = new List<IInteractableEnvironment>();

        public event Action EnvironmentListChanged;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize() {
            if (s_instance == null) {
                new GameObject("Game Manager").AddComponent<GameManager>();
            }
        }
        private void Awake() {
            s_instance = this;
        }

        private void OnApplicationQuit() {
            DataPersistenceManager.Save();
        }
        private void OnDestroy() {
            s_instance = null;
        }

    }
}

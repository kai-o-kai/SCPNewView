using UnityEngine;
using SCPNewView.Saving;

namespace SCPNewView.Management {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance => s_instance;
        private static GameManager s_instance;

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

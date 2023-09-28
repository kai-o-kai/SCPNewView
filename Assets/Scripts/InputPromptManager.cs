using UnityEngine;
using UnityEngine.InputSystem;

namespace SCPNewView {
    public class InputPromptManager : MonoBehaviour {
        public static InputPromptManager Instance { get {
                if (s_instance == null) {
                    s_instance = Object.Instantiate(ReferenceManager.Current.InputPromptManagerPrefab);
                }
                return s_instance;
        } }
        private static InputPromptManager s_instance;

        private void Awake() {
            s_instance = this;
        }
    }
}
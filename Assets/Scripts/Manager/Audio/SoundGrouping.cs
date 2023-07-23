using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCPNewView.Audio {
    [CreateAssetMenu(menuName = "Scriptable Objects/Sound Grouping")]
    public class SoundGrouping : ScriptableObject {
        public Sound[] Sounds { get => sounds; }
        public Scene AssociatedScene { get {
                return SceneManager.GetSceneByName(belongsTo);
            }
        }

        [SerializeField] string belongsTo;
        [SerializeField] Sound[] sounds;

        private void Awake() {
            if (belongsTo == null) {
                belongsTo = name;
            }
        }
    }
}

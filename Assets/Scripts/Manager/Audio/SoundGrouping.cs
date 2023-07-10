using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCPNewView.Audio {
    public class SoundGrouping : ScriptableObject {
        [SerializeField] Scene belongsTo;
        [SerializeField] Sound[] sounds;
    }
}

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SCPNewView.Audio {
    /* TODO
     * - Make the sounds array go under a scriptableobject which stores the sounds array and the scene it gets used for
     * - Audio groups need to find themselves rather than be inspector injected
     */
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance { get {
                if (s_instance == null) {
                    s_instance = new GameObject("Audio Manager").AddComponent<AudioManager>();
                }
                return s_instance;
        } private set => s_instance = value; }
        static AudioManager s_instance;

        Sound[] sounds;

        AudioMixerGroup _sfxGroup;
        AudioMixerGroup _musicGroup;

        AudioMixer _mixer;

        void Awake() {
            _mixer = Resources.Load<AudioMixer>("mainMixer");
            _sfxGroup = _mixer.FindMatchingGroups("SFX")[0];
            _musicGroup = _mixer.FindMatchingGroups("Music")[0];

            SoundGrouping[] soundGroupings = Resources.LoadAll<SoundGrouping>("Sound Groupings");
            SoundGrouping soundsForThisManager = Array.Find(soundGroupings, check => check.AssociatedScene == gameObject.scene);
            sounds = soundsForThisManager.Sounds;

            if (soundsForThisManager == null) { Debug.LogError($"SCPNewView Audio Manager: Cannot find an associated sound grouping for this scene! Create a sound grouping for {gameObject.scene.ToString()}.", this);  }

            foreach (Sound toInitialize in sounds) {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                toInitialize.Initialize(newSource, _musicGroup, _sfxGroup);
            }
        }
        public void PlaySoundByName(string name) {
            FindSoundByName(name)?.Play();
        }
        public void StopSoundByName(string name) {
            FindSoundByName(name)?.Stop();
        }
        Sound FindSoundByName(string str) {
            Sound output = Array.Find(sounds, s => s.name == str);
            if (output == null) {
                Debug.LogWarning($"SCPNewView Audio Manager: Attempted to find sound by name \"{str}\" but does not exist!", this);
            }
            return output;
        }
        void OnDestroy() {
            Instance = null;    
        }
    }
}
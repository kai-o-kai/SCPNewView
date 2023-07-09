using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SCPNewView.Audio {
    /* TODO
     * - Make the sounds array go under a scriptableobject which stores the sounds array and the scene it gets used for
     * - Audio groups need to find themselves rather than be inspector injected
     * - Have the instance create itself on get and destroy itself ondestroy (which is called on scenechange)
     */
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance { get; private set; }

        [SerializeField] Sound[] sounds;
        [SerializeField] AudioMixerGroup sfxGroup;
        [SerializeField] AudioMixerGroup musicGroup;

        void Awake() {
            Instance = this;
            foreach (Sound toInitialize in sounds) {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                toInitialize.Initialize(newSource, musicGroup, sfxGroup);
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
    }
}
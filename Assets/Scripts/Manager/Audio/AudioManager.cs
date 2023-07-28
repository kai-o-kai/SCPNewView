using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SCPNewView.Audio {
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
            if (soundsForThisManager == null) { Debug.LogWarning($"SCPNewView Audio Manager: Cannot find an associated sound grouping for this scene! Create a sound grouping for {gameObject.scene.name}.", this); return; }
            sounds = soundsForThisManager.Sounds;


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
        public void PlaySoundAtPosition(string name, Vector2 pos) {
            FindSoundByName(name)?.PlayAtPosition(pos);
        }
        Sound FindSoundByName(string str) {
            if (string.IsNullOrWhiteSpace(str)) return null;

            Sound output = null;
            try {
                output = Array.Find(sounds, s => s.name == str);
            } catch (Exception err) {
                Debug.LogWarning(err.Message);
            }
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
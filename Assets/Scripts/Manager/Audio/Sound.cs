using UnityEngine;
using UnityEngine.Audio;

namespace SCPNewView.Audio {
    [CreateAssetMenu(menuName = "ScriptableObjects/Sound", order = 5)]
    public class Sound : ScriptableObject {
        [SerializeField] AudioClip clip;
        [SerializeField] float volume;
        [SerializeField] float pitch;
        [SerializeField] bool loop;
        [SerializeField] bool playOnAwake;
        [SerializeField] SoundType type;
        AudioSource _source;

        public void Initialize(AudioSource source, AudioMixerGroup musicGroup, AudioMixerGroup sfxGroup) {
            _source = source;
            _source.volume = volume;
            _source.pitch = pitch;
            _source.loop = loop;
            _source.clip = clip;
            switch (type) {
                case SoundType.SFX:
                    _source.outputAudioMixerGroup = sfxGroup;
                    break;
                case SoundType.Music:
                    _source.outputAudioMixerGroup = musicGroup;
                    break;
            }
            if (playOnAwake) _source.Play();
        }
        public void Play() => _source.Play();
        public void Stop() => _source.Stop();
    }
}

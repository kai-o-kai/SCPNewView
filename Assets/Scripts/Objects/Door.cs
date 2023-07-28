using System;
using SCPNewView.Audio;
using SCPNewView.Inventory;
using UnityEngine;

namespace SCPNewView.Environment {
    [RequireComponent(typeof(Animator))]
    public abstract class Door : MonoBehaviour {
        public event Action Opened;
        public event Action Closed;

        protected const string ANIMDOOROPEN = "openDoor";
        protected const string ANIMDOORCLOSE = "closeDoor";

        protected virtual string SOUNDDOORTOGGLE { private get; set; } = "door_toggle";

        private Animator _animator;
        private bool _doorIsOpen;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }
        protected void ToggleDoor() {
            AudioManager.Instance.PlaySoundByName(SOUNDDOORTOGGLE);
            if (_doorIsOpen) {
                CloseDoor();
            } else {
                OpenDoor();
            }
            _doorIsOpen = !_doorIsOpen;
        }
        private void OpenDoor() {
            _animator.Play(ANIMDOOROPEN);
            Opened?.Invoke();
        }
        private void CloseDoor() {
            _animator.Play(ANIMDOORCLOSE);
            Closed?.Invoke();
        }
    }
}

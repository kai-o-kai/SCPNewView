using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Inventory {
    public class AutomaticStandardFirearm : IEquippableItem {
        public bool CanDeEquip { get; private set; }
        string _equipSound;
        string _fireSound;
        int _ammoPerMag;
        int _currentAmmo;
        int _roundsPerMinute;
        float _reloadTimeSeconds;
        AmmoType _ammoType;

        public AutomaticStandardFirearm(string equipSound, string fireSound, int ammoPerMag, int currentAmmo, int roundsPerMinute, float reloadTimeSeconds, AmmoType ammoType) {
            _equipSound = equipSound;
            _fireSound = fireSound;
            _ammoPerMag = ammoPerMag;
            _currentAmmo = currentAmmo;
            _roundsPerMinute = roundsPerMinute;
            _reloadTimeSeconds = reloadTimeSeconds;
            _ammoType = ammoType;
        }

        public void OnEquip() {
        }

        public void OnFireKeyHold() {
        }

        public void OnReloadKeyPress() {
        }
    }
    public enum AmmoType {
        A9mm, A556, A762, ABuckshot
    }
}
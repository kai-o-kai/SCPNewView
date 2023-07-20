using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCPNewView.Audio;
using UnityEngine;

namespace SCPNewView.Inventory {
    public class AutomaticStandardFirearm : IEquippableItem {
        public bool CanDeEquip { get; private set; }

        private string _equipSound;
        private string _fireSound;
        private int _ammoPerMag;
        private int _currentAmmo;
        private float _secondsBetweenShots;
        private float _reloadTimeSeconds;
        private AmmoType _ammoType;

        public AutomaticStandardFirearm(string equipSound, string fireSound, int ammoPerMag, int currentAmmo, int roundsPerMinute, float reloadTimeSeconds, AmmoType ammoType) {
            _equipSound = equipSound;
            _fireSound = fireSound;
            _ammoPerMag = ammoPerMag;
            _currentAmmo = currentAmmo;
            _secondsBetweenShots = (roundsPerMinute / 3600f);
            _reloadTimeSeconds = reloadTimeSeconds;
            _ammoType = ammoType;
        }

        public void OnEquip() {
            AudioManager.Instance.PlaySoundByName(_equipSound);
        }
        public void OnDeEquip() {

        }
        public void OnFireKeyStart() {
        }
        public void OnFireKeyEnd() {

        }

        public async void OnReloadKeyPress() {
            CanDeEquip = false;
            await Task.Delay(Mathf.RoundToInt(_reloadTimeSeconds * 1000));
            // reload
            CanDeEquip = true;
        }
    }
    public enum AmmoType {
        A9mm, A556, A762, ABuckshot
    }
}
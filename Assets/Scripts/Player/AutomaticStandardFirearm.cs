using System.Threading.Tasks;
using SCPNewView.Audio;
using SCPNewView.Utils;
using UnityEngine;

namespace SCPNewView.Inventory.InventoryItems {
    public class AutomaticStandardFirearm : IEquippableItem {
        public bool CanDeEquip { get; private set; }

        private string _equipSound;
        private string _fireSound;
        private int _ammoPerMag;
        private int _currentAmmo;
        private float _secondsBetweenShots;
        private float _reloadTimeMilliseconds;
        private AmmoType _ammoType;
        
        private bool _fireKeyIsPressed;
        private Timer _shootTimer;

        public AutomaticStandardFirearm(string equipSound, string fireSound, int ammoPerMag, int currentAmmo, int roundsPerMinute, float reloadTimeSeconds, AmmoType ammoType) {
            _equipSound = equipSound;
            _fireSound = fireSound;
            _ammoPerMag = ammoPerMag;
            _currentAmmo = currentAmmo;
            _secondsBetweenShots = (roundsPerMinute / 3600f);
            _reloadTimeMilliseconds = reloadTimeSeconds * 1000f;
            _ammoType = ammoType;
        }

        public void OnEquip() {
            AudioManager.Instance.PlaySoundByName(_equipSound);
        }
        public void OnDeEquip() {

        }
        public void OnFireKeyStart() {
            _fireKeyIsPressed = true;
            Fire();
        }
        public void OnFireKeyEnd() {
            Debug.Log("Fire key released");
            _fireKeyIsPressed = false;
            _shootTimer = null;
            Timer.RemoveTimersWithCallback(Fire);
        }
        public async void OnReloadKeyPress() {
            CanDeEquip = false;
            await Task.Delay(Mathf.RoundToInt(_reloadTimeMilliseconds));
            // reload
            CanDeEquip = true;
        }
        private void Fire() {
            if (!_fireKeyIsPressed) return;
            Debug.Log("gun go shoot");
            PrepareNextShot();
        }
        private void PrepareNextShot() {
            Debug.Log("Prepping next shot");
            _shootTimer = new Timer(Fire, _secondsBetweenShots);
            Debug.Log($"Shoot Timer set with {_shootTimer.SecondsLeft}s duration");
        }
    }
    public enum AmmoType {
        A9mm, A556, A762, ABuckshot
    }
}
using System;
using System.Threading.Tasks;
using SCPNewView.Audio;
using SCPNewView.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SCPNewView.Inventory.InventoryItems {
    public class SemiAutomaticStandardFirearm : IEquippableItem {
        public bool CanDeEquip { get; private set; } = true;
        public int Slot { get; private set; }

        private string _equipSound;
        private string _fireSound;
        private int _ammoPerMag;
        private int _currentAmmo;
        private float _secondsBetweenShots;
        private float _reloadTimeMilliseconds;
        private AmmoType _ammoType;
        private float _damage;

        private GameObject _bulletPrefab;
        private Transform _firePoint;

        private bool _readyToFire = true;

        public SemiAutomaticStandardFirearm(string equipSound = default, string fireSound = default, int ammoPerMag = 30, int roundsPerMinute = 50, float reloadTimeSeconds = 2.5f, AmmoType ammoType = AmmoType.A9mm, float damage = 60f, int slot = 2) {
            _equipSound = equipSound;
            _fireSound = fireSound;
            _ammoPerMag = ammoPerMag;
            _currentAmmo = _ammoPerMag;
            _secondsBetweenShots = (roundsPerMinute / 3600f);
            _reloadTimeMilliseconds = reloadTimeSeconds * 1000f;
            _ammoType = ammoType;
            _bulletPrefab = ReferenceManager.Current.BulletPrefab;
            _damage = damage;
            Slot = slot;

            Transform player = Object.FindObjectOfType<PlayerMovement>().transform;
            _firePoint = player.GetChild(0);
        }

        public void OnEquip() {
            AudioManager.Instance.PlaySoundByName(_equipSound);
        }
        public void OnDeEquip() {

        }
        public void OnFireKeyStart() {
            if (!_readyToFire) return;
            Fire();
        }
        public void OnFireKeyEnd() {
        }
        public async void OnReloadKeyPress() {
            CanDeEquip = false;
            await Task.Delay(Mathf.RoundToInt(_reloadTimeMilliseconds));
            // reload
            CanDeEquip = true;
        }
        private void Fire() {
            if (_currentAmmo <= 0) return; // TODO : Dryfire Sound
            AudioManager.Instance.PlaySoundByName(_fireSound);
            Object.Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation).GetComponent<Bullet>().Init(20f, Layers.PlayerFiredBullet, _damage);
            _readyToFire = false;
            _currentAmmo--;
            new Timer(ResetFireCooldown, _secondsBetweenShots);
        }
        private void ResetFireCooldown() => _readyToFire = true;

        public string SaveData() {
            // SAVE FORMAT : currentAmmo
            string output = $"{_currentAmmo}";
            return output;
        }

        public void LoadData(string data) {
            _currentAmmo = int.Parse(data);
        }
    }
}
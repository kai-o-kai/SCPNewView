using System.Threading.Tasks;
using SCPNewView.Audio;
using SCPNewView.Utils;
using UnityEngine;

namespace SCPNewView.Inventory.InventoryItems {
    public class AutomaticStandardFirearm : IEquippableItem {
        public bool CanDeEquip { get; private set; } = true;
        public int Slot { get; private set; } = 1;

        private string _equipSound;
        private string _fireSound;
        private int _ammoPerMag;
        private int _currentAmmo;
        private float _secondsBetweenShots;
        private float _reloadTimeMilliseconds;
        private AmmoType _ammoType;
        private float _damage;
        
        private bool _fireKeyIsPressed;
        private GameObject _bulletPrefab;
        private Transform _firePoint;

        public AutomaticStandardFirearm(string equipSound = default, string fireSound = default, int ammoPerMag = 30, int roundsPerMinute = 50, float reloadTimeSeconds = 2.5f, AmmoType ammoType = AmmoType.A762, float damage = 60f) {
            _equipSound = equipSound;
            _fireSound = fireSound;
            _ammoPerMag = ammoPerMag;
            _currentAmmo = _ammoPerMag;
            _secondsBetweenShots = (roundsPerMinute / 3600f);
            _reloadTimeMilliseconds = reloadTimeSeconds * 1000f;
            _ammoType = ammoType;
            _bulletPrefab = ReferenceManager.Current.BulletPrefab;
            _damage = damage;

            Transform player = Object.FindObjectOfType<PlayerMovement>().transform;
            _firePoint = player.GetChild(0);
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
            _fireKeyIsPressed = false;
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
            AudioManager.Instance.PlaySoundByName(_fireSound);
            Object.Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation).GetComponent<Bullet>().Init(20f, Layers.PlayerFiredBullet, _damage);
            PrepareNextShot();
        }
        private void PrepareNextShot() => new Timer(Fire, _secondsBetweenShots);
    }
    public enum AmmoType {
        A9mm, A556, A762, ABuckshot
    }
}
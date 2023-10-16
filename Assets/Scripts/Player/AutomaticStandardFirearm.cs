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
        private string _reloadSound;
        private int _ammoPerMag;
        private int _currentAmmo;
        private float _secondsBetweenShots;
        private float _reloadTimeMilliseconds;
        private AmmoType _ammoType;
        private float _damage;
        
        private bool _fireKeyIsPressed;
        private GameObject _bulletPrefab;
        private Transform _firePoint;

        private bool _magAvailable => PlayerInventory.Instance.MagazineCountDic[_ammoType] > 0;

        public AutomaticStandardFirearm(string equipSound = default, string fireSound = default, string reloadSound = default, int ammoPerMag = 30, int roundsPerMinute = 292, float reloadTimeSeconds = 2.5f, AmmoType ammoType = AmmoType.A762, float damage = 60f) {
            _equipSound = equipSound;
            _fireSound = fireSound;
            _ammoPerMag = ammoPerMag;
            _currentAmmo = _ammoPerMag;
            _secondsBetweenShots = 1 / (roundsPerMinute / 60f);
            _reloadTimeMilliseconds = reloadTimeSeconds * 1000f;
            _ammoType = ammoType;
            _bulletPrefab = ReferenceManager.Current.BulletPrefab;
            _damage = damage;
            _reloadSound = reloadSound;

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
            //AudioManager.Instance.StopSoundByName(_fireSound);
        }
        public async void OnReloadKeyPress() {
            if (!_magAvailable) {
                // TODO : Maybe this should play a sound or alert for no mags left?
                return;
            }
            CanDeEquip = false;
            await Task.Delay(Mathf.RoundToInt(_reloadTimeMilliseconds));
            AudioManager.Instance.PlaySoundByName(_reloadSound);
            PlayerInventory.Instance.MagazineCountDic[_ammoType] -= 1;
            _currentAmmo = _ammoPerMag;
            CanDeEquip = true;
        }
        private void Fire() {
            if (!_fireKeyIsPressed) return;
            if (_currentAmmo <= 0) return; // TODO : Dryfire sound
            Object.Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation).GetComponent<Bullet>().Init(20f, Layers.PlayerFiredBullet, _damage);
            AudioManager.Instance.PlaySoundByName(_fireSound);
            _currentAmmo--;
            PrepareNextShot();
        }
        private void PrepareNextShot() => new Timer(Fire, _secondsBetweenShots);
        public string SaveData() {
            string output = $"{_currentAmmo}";
            return output;
        }
        public void LoadData(string data) {
            if (string.IsNullOrWhiteSpace(data)) return;
            _currentAmmo = int.Parse(data);
        }
    }
    public enum AmmoType {
        A9mm, A556, A762, ABuckshot
    }
}
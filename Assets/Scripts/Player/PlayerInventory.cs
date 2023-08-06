using UnityEngine;
using SCPNewView.Inventory.InventoryItems;
using System;
using System.Collections.Generic;
using SCPNewView.Saving;
using SCPNewView.Utils;

namespace SCPNewView.Inventory {
    public class PlayerInventory : MonoBehaviour, IDataPersisting {
        public static PlayerInventory Instance { get; private set; }

        public IEquippableItem CurrentlyEquippedItem => _currentItem;
        public Dictionary<AmmoType, int> MagazineCountDic { get; private set; }

        private InputSettings _inputActions;

        private IEquippableItem _primarySlot;
        private IEquippableItem _secondarySlot;
        private IEquippableItem _tertiarySlot;

        private IEquippableItem _currentItem;

        private void Awake() {
            Instance = this;

            _inputActions = new InputSettings();
            _inputActions.Player.Fire.started += (ctx) => _currentItem?.OnFireKeyStart();
            _inputActions.Player.Fire.canceled += (ctx) => _currentItem?.OnFireKeyEnd();
            _inputActions.Player.PrimarySelect.performed += (ctx) => TrySelectQuickSlot(1);
            _inputActions.Player.SecondarySelect.performed += (ctx) => TrySelectQuickSlot(2);
            _inputActions.Player.TertiarySelect.performed += (ctx) => TrySelectQuickSlot(3);
            LoadSavedInventoryData();
            TrySelectQuickSlot(1);

            void LoadSavedInventoryData() {
                string primarySlotName = DataPersistenceManager.Current.PlayerData.PrimarySlot.Item;
                string secondarySlotName = DataPersistenceManager.Current.PlayerData.SecondarySlot.Item;
                string tertiarySlotName = DataPersistenceManager.Current.PlayerData.TertiarySlot.Item;
                if (!string.IsNullOrWhiteSpace(primarySlotName) && PrimarySlots.Items.ContainsKey(primarySlotName)) {
                    _primarySlot = PrimarySlots.Items[primarySlotName];
                }
                if (!string.IsNullOrWhiteSpace(secondarySlotName) && SecondarySlots.Items.ContainsKey(secondarySlotName)) {
                    _secondarySlot = SecondarySlots.Items[secondarySlotName];
                }
                if (!string.IsNullOrWhiteSpace(tertiarySlotName) && TertiarySlots.Items.ContainsKey(tertiarySlotName)) {
                    _tertiarySlot = TertiarySlots.Items[tertiarySlotName];
                }
                _primarySlot?.LoadData(DataPersistenceManager.Current.PlayerData.PrimarySlot.Data);
                _secondarySlot?.LoadData(DataPersistenceManager.Current.PlayerData.SecondarySlot.Data);
                _tertiarySlot?.LoadData(DataPersistenceManager.Current.PlayerData.TertiarySlot.Data);
                
                MagazineCountDic = DataPersistenceManager.Current.PlayerData.MagazineCountDic;
            }
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();
        private void TrySelectQuickSlot(int slot) {
            if (slot > 3 || slot < 1) throw new ArgumentOutOfRangeException(nameof(slot), slot, "Slot to select must be between 1 and 3!");
            if (_currentItem != null) {
                if (_currentItem.Slot == slot) return;
                if (!_currentItem.CanDeEquip) return;

                _currentItem.OnDeEquip();
            }
            switch (slot) {
                case 1: _currentItem = _primarySlot; break;
                case 2: _currentItem = _secondarySlot; break;
                case 3: _currentItem = _tertiarySlot; break;
            }
            _currentItem?.OnEquip();
        }
        private void OnDestroy() => Instance = null;
        public void OnGameSave() {
            DataPersistenceManager.Current.PlayerData.PrimarySlot = new PlayerInventorySlot() {
                Item = Functions.FindKeyFromValueDictionary(PrimarySlots.Items, _primarySlot),
                Data = _primarySlot?.SaveData()
            };
            DataPersistenceManager.Current.PlayerData.SecondarySlot = new() {
                Item = Functions.FindKeyFromValueDictionary(SecondarySlots.Items, _secondarySlot),
                Data = _secondarySlot?.SaveData()
            };
            DataPersistenceManager.Current.PlayerData.TertiarySlot = new() {
                Item = Functions.FindKeyFromValueDictionary(TertiarySlots.Items, _tertiarySlot),
                Data = _tertiarySlot?.SaveData()
            };
            Debug.Log(MagazineCountDic.ToString());
            DataPersistenceManager.Current.PlayerData.MagazineCountDic = MagazineCountDic;
        }
    }
}

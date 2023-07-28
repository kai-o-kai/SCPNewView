using UnityEngine;
using SCPNewView.Inventory.InventoryItems;
using System;
using SCPNewView.Saving;
using SCPNewView.Utils;

namespace SCPNewView.Inventory {
    public class PlayerInventory : MonoBehaviour, IDataPersisting {
        public static PlayerInventory Instance { get; private set; }

        public IEquippableItem CurrentlyEquippedItem => _currentItem;

        private InputSettings _inputActions;

        private IEquippableItem _primarySlot;
        private IEquippableItem _secondarySlot;
        private IEquippableItem _tertiarySlot;

        private IEquippableItem _currentItem;

        private void Awake() {
            Instance = this;

            _inputActions = new InputSettings();
            _inputActions.Player.Fire.started += (ctx) => _currentItem.OnFireKeyStart();
            _inputActions.Player.Fire.canceled += (ctx) => _currentItem.OnFireKeyEnd();
            _inputActions.Player.PrimarySelect.performed += (ctx) => TrySelectQuickSlot(1);
            _inputActions.Player.SecondarySelect.performed += (ctx) => TrySelectQuickSlot(2);
            _inputActions.Player.TertiarySelect.performed += (ctx) => TrySelectQuickSlot(3);
            try {
                _primarySlot = PrimarySlots.Items[DataPersistenceManager.Current.PlayerData.PrimarySlot];
                _secondarySlot = SecondarySlots.Items[DataPersistenceManager.Current.PlayerData.SecondarySlot];
                _tertiarySlot = TertiarySlots.Items[DataPersistenceManager.Current.PlayerData.TertiarySlot];
            } catch (Exception err) {
                Debug.LogWarning("Tried to find an empty item, dont worry about this too much");
            }

            TrySelectQuickSlot(1);
        }

        private void OnEnable() {
            _inputActions.Enable();
        }
        private void OnDisable() {
            _inputActions.Disable();
        }
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
        private void OnDestroy() {
            Instance = null;
        }
        public void OnGameSave() {
            DataPersistenceManager.Current.PlayerData.PrimarySlot = Functions.FindKeyFromValueDictionary<string, IEquippableItem>(PrimarySlots.Items, _primarySlot);
            DataPersistenceManager.Current.PlayerData.SecondarySlot = Functions.FindKeyFromValueDictionary<string, IEquippableItem>(SecondarySlots.Items, _secondarySlot);
            DataPersistenceManager.Current.PlayerData.TertiarySlot = Functions.FindKeyFromValueDictionary<string, IEquippableItem>(TertiarySlots.Items, _tertiarySlot); ;
        }
    }
}

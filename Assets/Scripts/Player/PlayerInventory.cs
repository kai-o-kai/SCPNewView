using UnityEngine;
using SCPNewView.Inventory.InventoryItems;
using System;

namespace SCPNewView.Inventory {
    public class PlayerInventory : MonoBehaviour {
        public IEquippableItem CurrentlyEquippedItem => _currentItem;

        private InputSettings _inputActions;

        private IEquippableItem _primarySlot;
        private IEquippableItem _secondarySlot;
        private IEquippableItem _tertiarySlot;

        private IEquippableItem _currentItem;

        private void Awake() {
            _inputActions = new InputSettings();
            _inputActions.Player.Fire.started += (ctx) => _currentItem.OnFireKeyStart();
            _inputActions.Player.Fire.canceled += (ctx) => _currentItem.OnFireKeyEnd();
            _inputActions.Player.PrimarySelect.performed += (ctx) => TrySelectQuickSlot(1);
            _inputActions.Player.SecondarySelect.performed += (ctx) => TrySelectQuickSlot(2);
            _inputActions.Player.TertiarySelect.performed += (ctx) => TrySelectQuickSlot(3);

            _primarySlot = new AutomaticStandardFirearm();
            _secondarySlot = new SemiAutomaticStandardFirearm();

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
    }
}

using UnityEngine;
using SCPNewView.Inventory.InventoryItems;

namespace SCPNewView.Inventory {
    public class PlayerInventory : MonoBehaviour {
        public IEquippableItem CurrentlyEquippedItem => _currentItem;

        private InputSettings _inputActions;
        private IEquippableItem _currentItem;

        private void Awake() {
            _inputActions = new InputSettings();
            _inputActions.Player.Fire.started += (ctx) => _currentItem.OnFireKeyStart();
            _inputActions.Player.Fire.canceled += (ctx) => _currentItem.OnFireKeyEnd();
            _currentItem = new AutomaticStandardFirearm(string.Empty, string.Empty, 30, 30, 50, 2.5f, AmmoType.A762);
        }
        private void OnEnable() {
            _inputActions.Enable();
        }
        private void OnDisable() {
            _inputActions.Disable();
        }
    }
}

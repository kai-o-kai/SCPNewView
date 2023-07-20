using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SCPNewView.Inventory {
    public class PlayerInventory : MonoBehaviour {
        public IEquippableItem CurrentlyEquippedItem => _currentItem;

        private InputSettings _inputActions;
        private IEquippableItem _currentItem;

        private void Awake() {
            _inputActions = new InputSettings();
            _inputActions.Player.Fire.started += (ctx) => _currentItem.OnFireKeyStart();
            _inputActions.Player.Fire.canceled += (ctx) => _currentItem.OnFireKeyEnd();
        }
        private void OnEnable() {
            _inputActions.Enable();
        }
        private void OnDisable() {
            _inputActions.Disable();
        }
    }
}

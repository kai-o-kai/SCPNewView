using System.Collections;
using System.Collections.Generic;
using SCPNewView.Environment;
using UnityEngine;

namespace SCPNewView.Inventory {
    public interface IEquippableItem {
        bool CanDeEquip { get; }
        int Slot { get; }

        bool HasAccessToDoor(Door door) => false;

        void OnEquip();
        void OnDeEquip();
        void OnFireKeyStart();
        void OnFireKeyEnd();
        void OnReloadKeyPress();
    }
    public class NullIEquippableItem : IEquippableItem {
        public bool CanDeEquip => true;

        public int Slot => 1;

        public bool HasAccessToDoor(Door door) => true;

        public void OnDeEquip() {
            Debug.LogWarning("Null IEquippable Item");
        }

        public void OnEquip() {
            Debug.LogWarning("Null IEquippable Item");
        }

        public void OnFireKeyEnd() {
            Debug.LogWarning("Null IEquippable Item");
        }

        public void OnFireKeyStart() {
            Debug.LogWarning("Null IEquippable Item");
        }

        public void OnReloadKeyPress() {
            Debug.LogWarning("Null IEquippable Item");
        }
    }
}
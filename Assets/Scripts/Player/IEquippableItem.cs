using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Inventory {
    public interface IEquippableItem {
        bool CanDeEquip { get; }
        int Slot { get; }

        bool HasAccessToDoor() => false;

        void OnEquip();
        void OnDeEquip();
        void OnFireKeyStart();
        void OnFireKeyEnd();
        void OnReloadKeyPress();
    }
}
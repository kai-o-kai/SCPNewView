using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Inventory {
    public interface IEquippableItem {
        bool CanDeEquip { get; }
        
        void OnEquip();
        void OnFireKeyHold();
        void OnReloadKeyPress();
    }
}
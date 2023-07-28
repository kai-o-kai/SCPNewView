using SCPNewView.Inventory;
using UnityEngine;

namespace SCPNewView.Environment {
    public interface IInteractableEnvironment {
        float InteractionDistance { get; }

        void Interact(PlayerInventory inventory);
    }
}
using System.Collections.Generic;
using SCPNewView.Environment;
using SCPNewView.Inventory;
using SCPNewView.Management;
using UnityEngine;

namespace SCPNewView {
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerEnvironmentInteract : MonoBehaviour {
        private PlayerInventory _inventory;
        private List<IInteractableEnvironment> _interactableEnvironmentPieces;

        private void Awake() {
            _inventory = GetComponent<PlayerInventory>();
        }
        private void Start() {
            GameManager.Instance.EnvironmentListChanged += OnEnvironmentListUpdate;
        }
        private void OnDestroy() {
            GameManager.Instance.EnvironmentListChanged -= OnEnvironmentListUpdate;
        }
        private void OnEnvironmentListUpdate() {
            _interactableEnvironmentPieces = GameManager.Instance.InteractableEnvironmentPieces;
        }
    }
}
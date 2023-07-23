using UnityEngine;

namespace SCPNewView {
    public class PlayerAiming : MonoBehaviour {
        [SerializeField] private Camera cam;
        private InputSettings _inputActions;

        private void Awake() {
            cam = cam == null ? Camera.main : cam;
            _inputActions = new InputSettings();
        }
        private void Update() {
            transform.rotation = Quaternion.Euler(0f, 0f, GetAimAngle());    
        }
        private float GetAimAngle() {
            Vector2 mousePosScreenSpace = _inputActions.Player.Aim.ReadValue<Vector2>();
            Vector2 mousePosWorldSpace = cam.ScreenToWorldPoint(mousePosScreenSpace);
            Vector2 dirToMouse = mousePosWorldSpace - (Vector2)transform.position;
            float angle = (Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg) - 90f;
            return angle;
        }
        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();
    }
}
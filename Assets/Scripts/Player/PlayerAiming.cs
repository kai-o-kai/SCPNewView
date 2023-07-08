using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SCPNewView {
    public class PlayerAiming : MonoBehaviour {
        [SerializeField] Camera cam;

        void Awake() {
            cam = cam == null ? Camera.main : cam;    
        }
        void Update() {
            transform.rotation = Quaternion.Euler(0f, 0f, GetAimAngle());    
        }

        // TODO : This needs to work with controllers too.
        float GetAimAngle() {
            Vector2 mousePosScreenSpace = Mouse.current.position.ReadValue();
            Vector2 mousePosWorldSpace = cam.ScreenToWorldPoint(mousePosScreenSpace);
            Vector2 dirToMouse = mousePosWorldSpace - (Vector2)transform.position;
            float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg - 90f;
            return angle;
        }
    }
}

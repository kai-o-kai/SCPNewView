using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace SCPNewView {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour {
        public static PlayerMovement Instance { get; private set; }

        public Vector2 ExternalForces {
            get => _externalForce;
            set => _externalForce += value;
        }

        Rigidbody2D _rb;
        InputSettings _inputActions;
        Vector2 _externalForce;

        [SerializeField] float moveSpeed;
        [SerializeField] float forceDamping = 1.2f;


        void Awake() {
            Instance = this;
            _rb = GetComponent<Rigidbody2D>();
            _inputActions = new InputSettings();
        }
        void Start() {
            _rb.gravityScale = 0f;
        }
        void Update() {
            DampExternalForces();
        }
        void FixedUpdate() {
            Vector2 moveVector = _inputActions.Player.Movement.ReadValue<Vector2>().normalized;
            moveVector *= moveSpeed;
            moveVector += _externalForce;
            _rb.velocity = moveVector;
        }
        void DampExternalForces() {
            _externalForce /= forceDamping;
            bool xIsLow = Mathf.Abs(_externalForce.x) < 0.01f;
            bool yIsLow = Mathf.Abs(_externalForce.y) < 0.01f;
            if (xIsLow && yIsLow) {
                _externalForce = Vector2.zero;
            }
        }
        void OnEnable() {
            _inputActions.Player.Enable();
        }
        void OnDisable() { 
            _inputActions.Player.Disable();
        }

        [ContextMenu("Fling External Forces")]
        void FlingInRandomDir() {
            Vector2 dir = new Vector2();
            dir.x = Random.Range(-15f, 15f);
            dir.y = Random.Range(-15f, 15f);
            ExternalForces = dir;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using SCPNewView.Saving;

namespace SCPNewView {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour, IDataPersisting {
        public static PlayerMovement Instance { get; private set; }

        public Vector2 ExternalForces {
            get => _externalForce;
            set => _externalForce += value;
        }

        private Rigidbody2D _rb;
        private InputSettings _inputActions;
        private Vector2 _externalForce;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float forceDamping = 1.2f;

        private void Awake() {
            Instance = this;
            _rb = GetComponent<Rigidbody2D>();
            _inputActions = new InputSettings();
        }

        private void Start() {
            _rb.gravityScale = 0f;
            _rb.position = DataPersistenceManager.Current.PlayerData.Position;
        }

        private void Update() => DampExternalForces();

        private void FixedUpdate() {
            Vector2 moveVector = _inputActions.Player.Movement.ReadValue<Vector2>().normalized;
            moveVector *= moveSpeed;
            moveVector += _externalForce;
            _rb.velocity = moveVector;
        }

        private void DampExternalForces() {
            _externalForce /= forceDamping;
            bool xIsLow = Mathf.Abs(_externalForce.x) < 0.01f;
            bool yIsLow = Mathf.Abs(_externalForce.y) < 0.01f;
            if (xIsLow && yIsLow) {
                _externalForce = Vector2.zero;
            }
        }

        private void OnEnable() {
            _inputActions.Player.Enable();
        }

        private void OnDisable() {
            _inputActions.Player.Disable();
        }

        [ContextMenu("Fling External Forces")]
        private void FlingInRandomDir() {
            Vector2 dir = new Vector2();
            dir.x = Random.Range(-15f, 15f);
            dir.y = Random.Range(-15f, 15f);
            ExternalForces = dir;
        }
        public void OnGameSave() {
            DataPersistenceManager.Current.PlayerData.Position = _rb.position;
        }
    }
}

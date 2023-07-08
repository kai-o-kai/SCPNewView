using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    public class DropShadow : MonoBehaviour {
        [SerializeField] VisualData data;

        Transform _copiedObject;
        SpriteRenderer _spriteRenderer;

        void Awake() {
            _copiedObject = transform.parent;        
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = new Color(0, 0, 0, 0.5f);
        }
        void Update() {
            transform.position = (Vector2)_copiedObject.position + data.DropShadowOffset;
            transform.rotation = _copiedObject.rotation;
        }
    }
}

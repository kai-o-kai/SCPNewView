using UnityEngine;

namespace SCPNewView {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour {
        private Rigidbody2D _rb;

        public void Init(float speed, int bulletLayer) {
            _rb = GetComponent<Rigidbody2D>();
            gameObject.layer = bulletLayer;
            _rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
        }
    }
}
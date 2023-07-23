using UnityEngine;

namespace SCPNewView {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour {
        private Rigidbody2D _rb;
        private float _damage;

        public void Init(float speed, int bulletLayer, float damage) {
            _rb = GetComponent<Rigidbody2D>();
            _damage = damage;
            gameObject.layer = bulletLayer;
            _rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
        }
        private void OnTriggerEnter2D(Collider2D other) {
            // Guard clauses for tool colliders go here

            if (other.TryGetComponent<IDamagable>(out var damagable)) {
                damagable.OnHitByBullet(this, _damage, gameObject.layer);
            } else {
                Destroy(gameObject);
            }
        }
    }
}
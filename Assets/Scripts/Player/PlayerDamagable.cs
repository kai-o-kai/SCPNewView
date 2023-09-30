using System.Collections;
using System.Collections.Generic;
using SCPNewView.Entities.SCP173;
using UnityEngine;

namespace SCPNewView {
    public class PlayerDamagable : MonoBehaviour, IDamagable {
        public void OnHitByBullet(Bullet b, float damage, int layer) {
            Debug.Log("Player hit by bullet.", this);
        }

        public void OnSnapBy173(SCP173 scp173) {
            Debug.Log("Player neck snapped by 173.", this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using SCPNewView.Entities.SCP173;
using UnityEngine;

namespace SCPNewView {
    public interface IDamagable {
        void OnHitByBullet(Bullet b, float damage, int layer);
        void OnSnapBy173(SCP173 scp173);
    }
}

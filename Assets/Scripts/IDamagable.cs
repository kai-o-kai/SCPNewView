using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    public interface IDamagable {
        void OnHitByBullet(Bullet b);
    }
}

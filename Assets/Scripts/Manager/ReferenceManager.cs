using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    [CreateAssetMenu(fileName = "New Reference Manager", menuName = "Scriptable Objects/Reference Manager")]
    public class ReferenceManager : ScriptableObject {
        #region Instance
        public static ReferenceManager Current {
            get {
                if (s_current == null) {
                    Debug.LogWarning("No reference manager created! Creating a blank instance.");
                    s_current = CreateInstance<ReferenceManager>();
                }
                return s_current;
            }
        }
        private static ReferenceManager s_current;
        #endregion

        public GameObject BulletPrefab => bulletPrefab;

        [SerializeField] private GameObject bulletPrefab;

        private void Awake() {
            if (s_current == null) {
                s_current = this;
            } else {
                Debug.LogWarning($"More than one {GetType()} in assets!");
            }
        }


    }
}
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using SCPNewView.Utils;
using UnityEditor;
using UnityEngine;

namespace SCPNewView {
    [CreateAssetMenu(fileName = "New Reference Manager", menuName = "Scriptable Objects/Reference Manager")]
    public class ReferenceManager : ScriptableObject {
        #region Instance
        public static ReferenceManager Current {
            get {        
                ReferenceManager loaded = Resources.Load<ReferenceManager>("Reference Manager");
                if (loaded == null) {
                    Debug.LogWarning("No reference manager created! Create one and place in the Resources folder under the name \"Reference Manager\"");
                }
                s_current = loaded;
                return s_current;
            }
        }
        private static ReferenceManager s_current;
        #endregion

        public GameObject BulletPrefab => bulletPrefab;
        public List<Tag> FriendlyEntityTags => friendlyEntityTags;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private List<Tag> friendlyEntityTags;
        [field: SerializeField] public Color MinFloorTileColor { get; private set; }
        [field: SerializeField] public Color MaxFloorTileColor { get; private set; }
    }
}
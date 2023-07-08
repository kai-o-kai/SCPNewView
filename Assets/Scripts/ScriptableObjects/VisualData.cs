using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    [CreateAssetMenu(menuName  = "Scriptable Objects/Visual Data", fileName = "New Visual Data")]
    public class VisualData : ScriptableObject {
        public Vector2 DropShadowOffset { get => dropShadowOffset;  }

        [SerializeField] Vector2 dropShadowOffset;
    }
}

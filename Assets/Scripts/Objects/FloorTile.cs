using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    [RequireComponent(typeof(SpriteRenderer))]
    public class FloorTile : MonoBehaviour {
        SpriteRenderer _sR;

        private void Awake() {
            _sR = GetComponent<SpriteRenderer>();
        }
        private void Start() {
            Color min = ReferenceManager.Current.MinFloorTileColor;
            Color max = ReferenceManager.Current.MaxFloorTileColor;

            Color color = Color.Lerp(min, max, Random.value);
            _sR.color = color;
        }
    }
}

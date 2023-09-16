using System.Collections.Generic;
using SCPNewView.Utils;
using UnityEngine;

namespace SCPNewView {
    public class Looker : MonoBehaviour {
        public static List<ILookable> Lookables = new List<ILookable>();

        [Range(0f, 360f)] [SerializeField] private float _fov;
        [SerializeField] private float _lookDistance;


        private void Awake() {
            AddLooker(this);
        }
        private void OnDestroy() {
            RemoveLooker(this);
        }
        private void Update() {
            
        }
        private void OnValidate() {
            _lookDistance = Mathf.Clamp(_lookDistance, 0, Mathf.Infinity);
        }
        private void OnDrawGizmosSelected() {
            float zAngle = transform.eulerAngles.z;
            float minAngle = zAngle - (_fov / 2);
            float maxAngle = zAngle + (_fov / 2);
            maxAngle += 90;
            minAngle += 90;
            minAngle = Utilities.Clamp0360(minAngle);
            maxAngle = Utilities.Clamp0360(maxAngle);
            Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(minAngle * Mathf.Deg2Rad), Mathf.Sin(minAngle * Mathf.Deg2Rad), 0) * _lookDistance);
            Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(maxAngle * Mathf.Deg2Rad), Mathf.Sin(maxAngle * Mathf.Deg2Rad), 0) * _lookDistance);
        }

        private static void AddLooker(Looker l) {
            foreach (var lookable in Lookables) {
                lookable.IsLookedAtBy.Add(l, false);
            }
        }
        private static void RemoveLooker(Looker l) {
            foreach (var lookable in Lookables) {
                lookable.IsLookedAtBy.Remove(l);
            }
        }

    }
}
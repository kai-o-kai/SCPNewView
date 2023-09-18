using System.Collections.Generic;
using SCPNewView.Utils;
using UnityEngine;

namespace SCPNewView {
    public class Looker : MonoBehaviour {
        public static List<Transform> Lookables = new List<Transform>();

        public bool Enabled { get; set; } = true;

        [Range(0f, 360f)] [SerializeField] private float _fov;
        [SerializeField] private float _lookDistance;

        private void Start() {
            AddLooker(this);
        }
        private void OnDestroy() {
            RemoveLooker(this);
        }
        private void Update() {
            float zAngle = transform.eulerAngles.z;
            float minAngle = zAngle - (_fov / 2);
            float maxAngle = zAngle + (_fov / 2);
            maxAngle += 90;
            minAngle += 90;
            minAngle = Utilities.Clamp0360(minAngle);
            maxAngle = Utilities.Clamp0360(maxAngle);
            foreach (var lookable in Lookables) {
                if (Enabled) {
                    float distance = Vector2.Distance(transform.position, lookable.position);
                    ILookable lightableObjectInterface = lookable.GetComponent<ILookable>();
                    if (distance > _lookDistance) {
                        lightableObjectInterface.IsLookedAtBy[this] = false;
                    } else {
                        Vector2 dirToLightable = lookable.position - transform.position;
                        float angleToLightable = Utilities.DirToAngle(dirToLightable) + 90f;
                        angleToLightable = Utilities.Clamp0360(angleToLightable);
                        bool isInLookFOV = (angleToLightable < maxAngle) && (angleToLightable > minAngle);
                        lightableObjectInterface.IsLookedAtBy[this] = isInLookFOV;
                    }
                } else {
                    ILookable lightableObjectInterface = lookable.GetComponent<ILookable>();
                    lightableObjectInterface.IsLookedAtBy[this] = false;
                }
            }
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
                lookable.GetComponent<ILookable>().IsLookedAtBy.Add(l, false);
            }
        }
        private static void RemoveLooker(Looker l) {
            foreach (var lookable in Lookables) {
                lookable.GetComponent<ILookable>().IsLookedAtBy.Remove(l);
            }
        }

    }
}
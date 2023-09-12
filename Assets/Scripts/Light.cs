using System.Collections.Generic;
using UnityEngine;
using LightType = UnityEngine.Rendering.Universal.Light2D.LightType;
using UnityEngine.Rendering.Universal;
using System;
using SCPNewView.Utils;

namespace SCPNewView {
    [RequireComponent(typeof(Light2D))]
    public class Light : MonoBehaviour {
        public static Light[] Lights => s_lights.ToArray();
        public static event Action LightListChanged;

        private static List<Light> s_lights = new List<Light>();
        
        private Light2D _light;
        private LightType _type;
        private Transform[] _lightableObjects;

        private void Awake() {
            _light = GetComponent<Light2D>();
            _type = _light.lightType;
            s_lights.Add(this);
            LightListChanged?.Invoke();
        }
        private void Start() {
            _lightableObjects = ILightable.GetLightableObjects();
            ILightable.LightableObjectsListChanged += OnLightableObjectsListChanged;
            if (_type == LightType.Global) {
                foreach (var lightable in _lightableObjects) {
                    lightable.GetComponent<ILightable>().IsLitBy[this] = true;
                    continue;
                }
            }
        }
        private void Update() {
            if (_type == LightType.Global) return; // We only ever need to perform global light checks once and we do it in start, so don't run it in update.
            if (_type == LightType.Point) { // Point == Spot
                float fov = _light.pointLightOuterAngle; // We use outers because even a little light counts as being lit by the light.
                float radius = _light.pointLightOuterRadius;
                float zAngle = transform.eulerAngles.z;
                float minAngle = zAngle + (fov / 2);
                float maxAngle = zAngle - (fov / 2);
                foreach (var lightable in _lightableObjects) {
                    float distance = Vector2.Distance(transform.position, lightable.position);
                    ILightable lightableObjectInterface = lightable.GetComponent<ILightable>();
                    if (distance > radius) {
                        lightableObjectInterface.IsLitBy[this] = false;
                    } else {
                        Vector2 dirToLightable = lightable.position - transform.position;
                        float angleToLightable = Utilities.DirToAngle(dirToLightable);
                        bool isInLightFOV = (angleToLightable < maxAngle) && (angleToLightable > minAngle);
                        lightableObjectInterface.IsLitBy[this] = isInLightFOV;
                    }
                }
            }
        }
        private void OnDestroy() {
            ILightable.LightableObjectsListChanged -= OnLightableObjectsListChanged;
            LightListChanged?.Invoke();
            s_lights.Remove(this);
        }
        private void OnLightableObjectsListChanged() => _lightableObjects = ILightable.GetLightableObjects();
    }
}
namespace SCPNewView.Utils {
    public static class Utilities {
        public static float DirToAngle(Vector2 dir) {
            float output = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            return output;
        }
    }
}
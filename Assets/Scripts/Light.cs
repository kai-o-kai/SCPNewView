using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightType = UnityEngine.Rendering.Universal.Light2D.LightType;
using UnityEngine.Rendering.Universal;
using System;

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
            if (_type == LightType.Global) {
                foreach (var lightable in _lightableObjects) {
                    lightable.GetComponent<ILightable>().IsLitBy[_light] = true;
                    continue;
                }
            }
        }
        private void Update() {
            if (_type == LightType.Global) return; // We only ever need to perform global light checks once and we do it in start, so don't run it in update.
            foreach (var lightable in _lightableObjects) {
                if (_type == LightType.Point) { // Point == Spot
                    // TODO : Angle checking
                }
            }
        }
        private void OnDestroy() {
            LightListChanged?.Invoke();
            s_lights.Remove(this);
        }
    }
}

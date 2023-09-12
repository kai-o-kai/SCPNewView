using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SCPNewView {
    public interface ILightable {
        private static List<Transform> s_lightableObjects = new List<Transform>();
        public static event Action LightableObjectsListChanged;

        Dictionary<Light, bool> IsLitBy { get; }

        public static void AddLightableObject(Transform newObj) {
           if (s_lightableObjects.Contains(newObj)) return;
           s_lightableObjects.Add(newObj);
           LightableObjectsListChanged?.Invoke();
        }
        public static void RemoveLightableObject(Transform toDestroy) {
            if (!s_lightableObjects.Contains(toDestroy)) return;
            s_lightableObjects.Remove(toDestroy);
            LightableObjectsListChanged?.Invoke();
        }
        public static Transform[] GetLightableObjects() => s_lightableObjects.ToArray();
    }
}

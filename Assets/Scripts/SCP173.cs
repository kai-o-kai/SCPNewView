using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCPNewView.Entities.SCP173 {
    public class SCP173 : MonoBehaviour, ILightable, ILookable {
        public Dictionary<Light, bool> IsLitBy { get; } = new Dictionary<Light, bool>();

        public Dictionary<Looker, bool> IsLookedAtBy { get; } = new Dictionary<Looker, bool>();

        private void Awake() {
            ILightable.AddLightableObject(transform);
            ILookable.AddLookable(this);
        }
        private void Start() {
            Light.LightListChanged += UpdateLightsDic;
        }
        private void OnDestroy() {
            ILightable.RemoveLightableObject(transform);
            ILookable.RemoveLookable(this);
            Light.LightListChanged -= UpdateLightsDic;
        }
        private void UpdateLightsDic() {
            Light[] lights = Light.Lights;
            // TODO : This function is hella expensive for no reason. Optimise.
            foreach (var toCheckCorresponding in lights) {
                if (!IsLitBy.ContainsKey(toCheckCorresponding)) {
                    IsLitBy.Add(toCheckCorresponding, false);
                }
            }
            foreach (var toCheckCorresponding in IsLitBy) {
                if (!lights.Contains(toCheckCorresponding.Key)) {
                    IsLitBy.Remove(toCheckCorresponding.Key);
                }
            }
        }
    }
}
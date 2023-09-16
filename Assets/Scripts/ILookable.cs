using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    public interface ILookable {
        public Dictionary<Looker, bool> IsLookedAtBy { get; }
        
        public static void AddLookable(Transform il) {
            Looker.Lookables.Add(il);
        }
        public static void RemoveLookable(Transform il) {
            Looker.Lookables.Remove(il);
        }
    }
}

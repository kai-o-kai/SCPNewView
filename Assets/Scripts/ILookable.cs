using System.Collections.Generic;

namespace SCPNewView {
    public interface ILookable {
        public Dictionary<Looker, bool> IsLookedAtBy { get; }
        
        public static void AddLookable(ILookable il) {
            Looker.Lookables.Add(il);
        }
        public static void RemoveLookable(ILookable il) {
            Looker.Lookables.Remove(il);
        }
    }
}

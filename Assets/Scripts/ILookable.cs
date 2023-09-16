using System.Collections.Generic;

namespace SCPNewView {
    public interface ILookable {
        public Dictionary<Looker, bool> IsLookedAtBy { get; }
        
        private static void AddLookable(ILookable il) {
            Looker.Lookables.Add(il);
        }
        private static void RemoveLookable(ILookable il) {
            Looker.Lookables.Remove(il);
        }
    }
}

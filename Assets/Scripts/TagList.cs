using System.Collections;
using System.Collections.Generic;
using SCPNewView.Utils;
using UnityEngine;

namespace SCPNewView {
    public class TagList : MonoBehaviour {
        [SerializeField] List<Tag> tags = new List<Tag>();

        public bool HasTag(Tag t) => tags.Contains(t);
        public bool HasAnyTag(Tag[] toCheck) {
            if (toCheck == null) return false;
            foreach (Tag t in toCheck) {
                if (tags.Contains(t)) return true;
            }
            return false;
        }
        public void AddTag(Tag toAdd) {
            if (tags.Contains(toAdd)) return;
            tags.Add(toAdd);
        }
        public void RemoveTag(Tag toRemove) {
            if (!tags.Contains(toRemove)) return;
            tags.Remove(toRemove);
        }
    }
}

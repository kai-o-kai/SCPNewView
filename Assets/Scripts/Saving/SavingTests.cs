using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Saving {
    public class SavingTests : MonoBehaviour {
        [SerializeField] SampleSaveClass toSave;

        [ContextMenu("Save")]
        void Save() {
            SaveManager.SaveGame(toSave, "TestSaveData1.save");
        }
        [ContextMenu("Load")]
        void Load() {
            SampleSaveClass loaded = (SampleSaveClass)SaveManager.LoadGame<SampleSaveClass>("TestSaveData1.save");
            Debug.Log(loaded.ToString());
        }
    }
    // TODO : This class needs to test members with other accessibility modifiers. Try private fields, auto-properties, serializedfields, etc.
    [System.Serializable]
    public class SampleSaveClass {
        public string a;
        public int b;
        public bool c;
        public KeyCode d;

        public SampleSaveClass(string a, int b, bool c, KeyCode d) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
        public override string ToString() {
            return $"A: {a}, B: {b}, C: {c}, D: {d.ToString()}";
        }
    }
}

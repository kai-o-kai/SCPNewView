using System.IO;
using UnityEngine;
using UnityEditor;

namespace SCPNewView {
    public class EditorTools : MonoBehaviour {

        [MenuItem("Tools/DeleteSave")]
        private static void DeleteSaveFiles() {
            string path = Application.persistentDataPath + "/Saves/Data/gameData.save";
            File.Delete(path);
        }
    }
}

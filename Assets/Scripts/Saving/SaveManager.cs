using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SCPNewView.Saving.Serialization;

namespace SCPNewView.Saving {
    public static class SaveManager {
        static ISerializer s_serializer;
        static string s_pathPrefix;

        public static void SaveGame(object toSave, string path) {
            s_serializer = s_serializer == null ? new LegacyJSONSerializer() : s_serializer;
            s_pathPrefix = s_pathPrefix == null ? Application.persistentDataPath + "/Saves/" : s_pathPrefix;
            try {
                string totalPath = Path.Combine(s_pathPrefix, path);
                string directoryPath = Path.GetDirectoryName(totalPath);
                if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }
                using (FileStream stream = new FileStream(totalPath, FileMode.Create)) {
                    using (StreamWriter writer = new StreamWriter(stream)) {
                        writer.Write(s_serializer.Serialize(toSave));
                    }
                }
            } catch (Exception err) {
                Debug.LogException(err);
                return;
            }
        }
        public static object LoadGame<T>(string path) {
            s_serializer = s_serializer == null ? new LegacyJSONSerializer() : s_serializer;
            s_pathPrefix = s_pathPrefix == null ? Application.persistentDataPath + "/Saves/" : s_pathPrefix;
            try {
                string totalPath = Path.Combine(s_pathPrefix, path);
                string directoryPath = Path.GetDirectoryName(totalPath);
                string loaded = "";
                if (!File.Exists(totalPath)) return null;
                if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }
                using (FileStream stream = new FileStream(totalPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        loaded = reader.ReadToEnd();
                    }
                }
                return s_serializer.Deserialize<T>(loaded);

            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
    }
}

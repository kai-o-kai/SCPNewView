using System;
using UnityEngine;

namespace SCPNewView.Saving.Serialization {
    public class LegacyJSONSerializer : ISerializer {
        public object Serialize(object toSerialize) {
            try {
                string output = JsonUtility.ToJson(toSerialize);
                return output;
            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
        public object Deserialize<T>(object toDeserialize) {
            try {
                object output = JsonUtility.FromJson<T>((string)toDeserialize);
                return output;
            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
    }
}

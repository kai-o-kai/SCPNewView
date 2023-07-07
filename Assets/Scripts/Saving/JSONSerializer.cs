using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SCPNewView.Saving.Serialization {
    public class JSONSerializer : ISerializer {
        public string Serialize(object toSerialize) {
            try {
                string output = JsonConvert.SerializeObject(toSerialize);
                return output;
            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
        public object Deserialize<T>(string toDeserialize) {
            try {
                T output = JsonConvert.DeserializeObject<T>(toDeserialize);
                return output;
            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
    }
}

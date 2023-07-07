using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SCPNewView.Saving.Serialization {
    /* JSON Serializer Overview
     * - Serialized private fields are NOT saved
     * - Properties, manual backing fields or not ARE saved
     * - Public fields ARE saved
     * - Non serialized private fields ARE saved
     */
    public class JSONSerializer : ISerializer {
        public object Serialize(object toSerialize) {
            try {
                string output = JsonConvert.SerializeObject(toSerialize);
                return output;
            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
        public object Deserialize<T>(object toDeserialize) {
            try {
                T output = JsonConvert.DeserializeObject<T>((string)toDeserialize);
                return output;
            } catch (Exception err) {
                Debug.LogException(err);
                return null;
            }
        }
    }
}

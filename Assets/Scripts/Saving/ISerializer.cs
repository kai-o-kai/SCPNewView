using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Saving.Serialization {
    public interface ISerializer {
// TODO : This interface should take in objects not strings so the interface can be used with other serializers like xml or binary.
        string Serialize(object toSerialize);
        object Deserialize<T>(string toDeserialize);
    }
}

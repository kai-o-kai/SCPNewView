using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Saving.Serialization {
    public interface ISerializer {
        string Serialize(object toSerialize);
        object Deserialize<T>(string toDeserialize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Saving.Serialization {
    public interface ISerializer {
        object Serialize(object toSerialize);
        object Deserialize<T>(object toDeserialize);
    }
}

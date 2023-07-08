using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCPNewView.Saving;

namespace SCPNewView.Management {
    public class GameManager : MonoBehaviour {
        void OnApplicationQuit() {
            DataPersistenceManager.Save();
        }
    }
}

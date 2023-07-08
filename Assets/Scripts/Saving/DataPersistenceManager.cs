using System;
using System.Collections;
using System.Collections.Generic;
using SCPNewView.Saving;
using UnityEngine;

namespace SCPNewView {
    public static class DataPersistenceManager {
        public static GameData Current {
            get {
                if (s_current == null) {
                    GameData loaded = (GameData)SaveManager.LoadGame<GameData>("Data/gameData");
                    if (loaded == null) { loaded = new GameData(); }
                    s_current = loaded;
                }
                return s_current;
            }
            private set {
                s_current = value;
            }
        }
        static GameData s_current;
    } 
    [Serializable]
    public class GameData {
        public PlayerData PlayerData { get; private set; }
        
        public GameData() {
            PlayerData = new PlayerData();
        }
    }
    [Serializable]
    public class PlayerData {
        public Vector2 Position { get; set; }

        public PlayerData() {
            // Load default stats from a scriptableobject
        }
    }
}

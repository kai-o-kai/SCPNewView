using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SCPNewView.Saving {
    public static class DataPersistenceManager {
        public static GameData Current {
            get {
                if (s_current == null) {
                    GameData loaded = (GameData)SaveManager.LoadGame<GameData>("Data/gameData.save");
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

        public static void Save() {
            List<IDataPersisting> objectsWithPersistingData = Object.FindObjectsOfType<MonoBehaviour>().OfType<IDataPersisting>().ToList();
            foreach (IDataPersisting toCall in objectsWithPersistingData) {
                toCall.OnGameSave();
            }
            SaveManager.SaveGame(s_current, "Data/gameData.save");
        }
    } 
    [Serializable]
    public class GameData {
        public PlayerData PlayerData { get => _playerData; }
        
        [SerializeField] PlayerData _playerData;
        
        public GameData() {
            _playerData = new PlayerData();
        }
    }
    [Serializable]
    public class PlayerData {
        public Vector2 Position { get => _position; set => _position = value; }

        [SerializeField] Vector2 _position;

        public PlayerData() {
            // Load default stats from a scriptableobject. This position setting is just placeholder.
            _position = new Vector2(1f, 2f);
        }
    }
    public interface IDataPersisting {
        void OnGameSave();
    }
}

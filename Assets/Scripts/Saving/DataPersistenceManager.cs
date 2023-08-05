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
        public SCP049Data Scp049Data { get => _scp049Data; }

        [SerializeField] PlayerData _playerData;
        [SerializeField] SCP049Data _scp049Data;

        public GameData() {
            _playerData = new PlayerData();
            _scp049Data = new SCP049Data();
        }
    }
    [Serializable]
    public class PlayerData {
        public Vector2 Position { get => _position; set => _position = value; }
        public PlayerInventorySlot PrimarySlot { get => _primarySlot; set => _primarySlot = value; }
        public PlayerInventorySlot SecondarySlot { get => _secondarySlot; set => _secondarySlot = value; }
        public PlayerInventorySlot TertiarySlot { get => _tertiarySlot; set => _tertiarySlot = value; }

        [SerializeField] Vector2 _position;
        [SerializeField] PlayerInventorySlot _primarySlot;
        [SerializeField] PlayerInventorySlot _secondarySlot;
        [SerializeField] PlayerInventorySlot _tertiarySlot;

        public PlayerData() {
            // Defaults Here
            _position = new Vector2(1f, 2f);
            _primarySlot = new PlayerInventorySlot();
            _secondarySlot = new PlayerInventorySlot();
            _tertiarySlot = new PlayerInventorySlot();
            _primarySlot.Item = "e11standardrifle";
            _secondarySlot.Item = "g19";
        }
    }
    [Serializable]
    public struct PlayerInventorySlot {
        public string Item;
        public string Data;
    }
    [Serializable]
    public class SCP049Data {
        public Vector2 Position { get => _position; set => _position = value; }

        [SerializeField] Vector2 _position;

        public SCP049Data() {
            // Defaults Here
            _position = new Vector2(3f, -2f);
        }
    }
    public interface IDataPersisting {
        void OnGameSave();
    }
}
namespace SCPNewView.Inventory.InventoryItems {
    public static class PrimarySlots {
        // TODO : This class might want to be refactored into a generic primaryitems class
        public static Dictionary<string, IEquippableItem> Items = new Dictionary<string, IEquippableItem>() {
            { "e11standardrifle", _e11StandardRifle }
        };
        // TODO : Fill this class
        private static IEquippableItem _e11StandardRifle => new AutomaticStandardFirearm("rifle_equip", "rifle_shoot");
    }
    public static class SecondarySlots {
        public static Dictionary<string, IEquippableItem> Items = new Dictionary<string, IEquippableItem>() {
            { "g19", _g19 }
        };
        private static IEquippableItem _g19 => new SemiAutomaticStandardFirearm();
    }
    public static class TertiarySlots {

        public static Dictionary<string, IEquippableItem> Items = new Dictionary<string, IEquippableItem>() {
        };
    }
}
namespace SCPNewView.Utils {
    public static class Functions {
        public static T FindKeyFromValueDictionary<T, K>(Dictionary<T, K> collection, K value) {
            KeyValuePair<T, K>[] pairs = collection.ToArray();
            object output = null;
            foreach (KeyValuePair<T, K> toCheck in pairs) {
                if (EqualityComparer<K>.Default.Equals(toCheck.Value, value)) {
                    output = toCheck.Key;
                    break;
                }
            }
            return (T)output;
        }
    }
}
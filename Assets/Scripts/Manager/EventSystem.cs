using System;

namespace SCPNewView.Management {
    public static class EventSystem {
        public static event Action NewEntitySpawned;

        /// <summary>
        /// This method should NOT be called in an entities Awake method. It should only be called by spawner objects after spawning a new entity.
        /// </summary>
        public static void OnNewEntitySpawned() => NewEntitySpawned?.Invoke();
    }
}

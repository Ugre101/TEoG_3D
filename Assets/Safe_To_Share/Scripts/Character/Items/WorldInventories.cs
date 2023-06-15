using System.Collections.Generic;
using Items;
using Safe_To_Share.Scripts.Building;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items {
    public static class WorldInventories {
        public static Dictionary<string, Inventory> Inventories = new();

        public static WorldInventoriesSave Save() => new(Inventories);

        public static void Load(WorldInventoriesSave toLoad) {
            Inventories = new Dictionary<string, Inventory>();
            if (toLoad.Saves == null || toLoad.Saves.Count == 0) return;
            foreach (var save in toLoad.Saves) {
                var inv = new Inventory();
                inv.Load(save.Save);
                if (!Inventories.TryAdd(save.Guid, inv))
                    Debug.LogWarning($"Failed to load chest with guid {save.Guid}");
            }
        }
    }
}
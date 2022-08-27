using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Building
{
    [Serializable]
    public struct WorldInventoriesSave
    {
        [Serializable]
        public struct WorldInventorySave
        {
            public WorldInventorySave(string guid, InventorySave save)
            {
                Guid = guid;
                Save = save;
            }

            public string Guid;
            public InventorySave Save;
        }
        
        [SerializeField] List<WorldInventorySave> saves;

        public WorldInventoriesSave(IEnumerable<KeyValuePair<string,Inventory>> pairs)
        {
            saves = new List<WorldInventorySave>();
            foreach (var (key, value) in pairs)
            {
                saves.Add(new WorldInventorySave(key,value.Save()));
            }
        }

        public List<WorldInventorySave> Saves => saves;
    }
}
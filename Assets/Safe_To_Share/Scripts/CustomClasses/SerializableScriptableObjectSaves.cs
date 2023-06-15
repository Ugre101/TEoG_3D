using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses {
    [Serializable]
    public struct SerializableScriptableObjectSaves {
        [SerializeField] List<string> savedGuids;

        public SerializableScriptableObjectSaves(IEnumerable<SerializableScriptableObject> objects) {
            savedGuids = new List<string>();
            foreach (var guid in objects)
                savedGuids.Add(guid.Guid);
        }

        public List<string> SavedGuids => savedGuids;
    }
}
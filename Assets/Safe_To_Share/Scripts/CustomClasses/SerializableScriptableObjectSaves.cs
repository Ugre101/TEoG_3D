using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses
{
    [System.Serializable]
    public struct SerializableScriptableObjectSaves
    {
        [SerializeField] List<string> savedGuids;

        public SerializableScriptableObjectSaves(IEnumerable<SerializableScriptableObject> objects)
        {
            savedGuids = new List<string>();
            foreach (SerializableScriptableObject guid in objects) 
                savedGuids.Add(guid.Guid);
        }

        public List<string> SavedGuids => savedGuids;
    }
}
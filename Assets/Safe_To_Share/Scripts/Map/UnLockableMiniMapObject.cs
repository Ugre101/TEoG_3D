using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    public sealed class UnLockableMiniMapObject : MiniMapBaseObject
    {
        [SerializeField] Collider coll;
        [SerializeField] string guid;
        [SerializeField] string areaName;
        public bool Showing { get; private set; }
        public event Action<UnLockableMiniMapObject> ShowMe; 
        public event Action<UnLockableMiniMapObject> StopShowingMe; 
# if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying) return;
            if (coll == null && !TryGetComponent(out coll)) 
                Debug.LogWarning($"No collider found on {name}");
            if (string.IsNullOrWhiteSpace(guid))
                guid = Guid.NewGuid().ToString();
        }
#endif

        public bool UnLocked => UnlockedMiniMapObjects.UnlockedObjects.Contains(guid);
        void Start()
        {
            if (UnLocked) 
                DisableCollider();
            UnlockedMiniMapObjects.UnLockAfterLoad += CheckOnLoad;
        }

        void OnDestroy() => UnlockedMiniMapObjects.UnLockAfterLoad -= CheckOnLoad;

        void CheckOnLoad(IEnumerable<string> obj)
        {
            if (obj.Contains(guid))
            {
                if (Showing) return;
                ShowOnMap();
            }
            else if (Showing) 
                HideOnMap();
        }

        void HideOnMap()
        {
            coll.enabled = true;
            Showing = false;
            StopShowingMe?.Invoke(this);
        }

        void ShowOnMap()
        {
            DisableCollider();
            ShowMe?.Invoke(this);
        }

        void DisableCollider()
        {
            coll.enabled = false;
            Showing = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (Showing)
            {
                DisableCollider();
                return;
            }
            if (!UnLocked) 
                UnlockedMiniMapObjects.UnlockedObjects.Add(guid);
            UnLock();
        }

        void UnLock()
        {
            EventLog.AddEvent($"Discovered {areaName}");
            ShowOnMap();
        }
    }

    public static class UnlockedMiniMapObjects
    {
        public static event Action<IEnumerable<string>> UnLockAfterLoad; 
        public static List<string> UnlockedObjects = new List<string>();

        public static void Load(UnLockableMiniMapObjectsSave savedGuids)
        {
            UnlockedObjects = new List<string>(savedGuids.UnlockedObjects);
            UnLockAfterLoad?.Invoke(savedGuids.UnlockedObjects);
        }
        
    }
    
    [Serializable]
    public struct UnLockableMiniMapObjectsSave
    {
        public UnLockableMiniMapObjectsSave(List<string> unlockedObjects) => this.unlockedObjects = unlockedObjects;
        [SerializeField] List<string> unlockedObjects;
        public List<string> UnlockedObjects => unlockedObjects;
    }
}
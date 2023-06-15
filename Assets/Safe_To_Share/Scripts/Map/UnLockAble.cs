using System;
using UnityEngine;

namespace Map {
    [Serializable]
    public class UnLockAble {
        [SerializeField] Material locked, unLocked;
        [SerializeField] MeshRenderer rend;
        public void UnLock() => rend.material = unLocked;

        public void Lock() => rend.material = locked;
    }
}
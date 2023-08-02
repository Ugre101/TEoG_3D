using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    [Serializable]
    public class UnLockAble {
        [SerializeField] Material locked, unLocked;
        [SerializeField] MeshRenderer rend;
        public void UnLock() => rend.material = unLocked;

        public void Lock() => rend.material = locked;
    }
}
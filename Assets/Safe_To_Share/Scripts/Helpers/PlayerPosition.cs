using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Helpers
{
    public sealed class PlayerPosition : MonoBehaviour
    {
        public static event Action<Vector3> PlayerMoved; 
        public static Vector3 Pos { get; private set; } = new();
        [SerializeField, Range(0.5f, 10f)] float distTolerance = 0.2f;
        [SerializeField,Range(float.Epsilon,0.5f)] float updateInterval = 1f;
        [SerializeField] Transform trans;
        float lastTick = 0;


#if UNITY_EDITOR
        void OnValidate()
        {
            if (trans == null)
                trans = transform;
        }
#endif
        
        void Update()
        {
            if (!(lastTick + updateInterval < Time.time)) return;
            lastTick = Time.time;
            var offset = Pos - trans.position;
            var dist = Vector3.SqrMagnitude(offset);
            if (distTolerance < dist)
            {
                Pos = trans.position;
                PlayerMoved?.Invoke(Pos);
            }

        }
    }
}
using System;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class EnemyZoneMiniMapObject : MiniMapBaseObject
    {
        [SerializeField, Range(0f, 1f),] float difficulty;
        public Color Color => new(difficulty, 0.4f, 0, 0.6f);
    }
}
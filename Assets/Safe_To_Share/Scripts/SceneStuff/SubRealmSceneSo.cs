using System;
using System.Collections.Generic;
using Character.EnemyStuff;
using UnityEngine;

namespace SaveStuff
{
    [CreateAssetMenu(menuName = "Scene Data/Sub Realm ref", fileName = "New Sub Realm SceneSo", order = 0)]
    public class SubRealmSceneSo : GameSceneSo
    {
        [Serializable]
        public struct SavedEnemy
        {
            [field: SerializeField] public Enemy Enemy { get; private set; }
            [field: SerializeField] public Vector3 Position { get; private set; }
            [field: SerializeField] public Quaternion Rotation { get; private set; }
        }

        public List<SavedEnemy> Enemies { get; set; }
    }
}
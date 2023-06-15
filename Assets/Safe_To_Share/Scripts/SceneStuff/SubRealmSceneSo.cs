using System;
using System.Collections.Generic;
using Character.EnemyStuff;
using SceneStuff;
using UnityEngine;

namespace SaveStuff {
    [CreateAssetMenu(menuName = "Scene Data/Sub Realm ref", fileName = "New Sub Realm SceneSo", order = 0)]
    public sealed class SubRealmSceneSo : GameSceneSo {
        [field: SerializeField] public SceneTeleportExit Exit { get; private set; }

        public Dictionary<string, List<SavedEnemy>> Enemies { get; } = new();

        [Serializable]
        public struct SavedEnemy {
            [field: SerializeField] public Enemy Enemy { get; private set; }
            [field: SerializeField] public Vector3 Position { get; private set; }
            [field: SerializeField] public Quaternion Rotation { get; private set; }

            public SavedEnemy(Enemy enemy, Vector3 position, Quaternion rotation) {
                Enemy = enemy;
                Position = position;
                Rotation = rotation;
            }
        }
    }
}
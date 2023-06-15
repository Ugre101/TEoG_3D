using System;
using System.Collections.Generic;
using System.Linq;
using Character.CreateCharacterStuff;
using Character.IslandData;

namespace Character.EnemyStuff {
    public static class SavedEnemies {
        static readonly Random Rng = new();

        public static Dictionary<int, List<Enemy>> Enemies = new();

        public static void ClearEnemies() {
            foreach (var e in Enemies.Values.SelectMany(list => list))
                e.NullifyRemoved();
            Enemies = new Dictionary<int, List<Enemy>>();
        }

        public static void SetupZone(int id, int spawnAmount, Islands island, params EnemyPreset[] enemyPresets) {
            var newEnemies = new List<Enemy>();
            for (var i = 0; i < spawnAmount; i++)
                CreateEnemy(id, enemyPresets, newEnemies, island);
            Enemies.Add(id, newEnemies);
        }

        static void CreateEnemy(int id, IReadOnlyList<EnemyPreset> presets, ICollection<Enemy> newEnemies,
                                Islands islands) {
            Enemy enemy = new(presets[Rng.Next(presets.Count)].NewEnemy(), islands);
            enemy.Removed += Removed;
            newEnemies.Add(enemy);

            void Removed() {
                enemy.Removed -= Removed;
                Enemies[id].Remove(enemy);
            }
        }
    }
}
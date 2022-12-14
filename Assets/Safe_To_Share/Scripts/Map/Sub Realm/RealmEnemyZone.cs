using System;
using System.Collections.Generic;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Map.Sub_Realm
{
    public class RealmEnemyZone : MonoBehaviour
    {
        [SerializeField] AssetReference enemyPrefab;
        [SerializeField] EnemyPreset[] enemyPresets;   
        [SerializeField] BossPreset bossPreset;
        [SerializeField] LayerMask spawnOn;
        [SerializeField] List<Vector3> spawnPoints = new();
        [SerializeField] Vector3 bossSpawnPoint;
        void OnTriggerEnter(Collider other)
        {
            if (gameObject.CompareTag("Player")) 
                SpawnEnemies();
        }

        void OnTriggerExit(Collider other)
        {
            if (gameObject.CompareTag("Player")) 
                DeSpawnEnemies();
        }

        void DeSpawnEnemies()
        {
            
            
        }

        static System.Random rng = new ();
        void SpawnEnemies()
        {
            SpawnBoss();
            if (enemyPresets.Length <= 0) return;
            foreach (var spawnPoint in spawnPoints)
            {
                Addressables.InstantiateAsync(enemyPrefab, spawnPoint, Quaternion.identity).Completed += op =>
                {
                    if (op.Status != AsyncOperationStatus.Succeeded) return;
                    if (!op.Result.TryGetComponent(out SubRealmEnemy enemyHolder)) return;
                    var enemyPreset = enemyPresets[rng.Next(0, enemyPresets.Length)];
                    enemyHolder.Setup(new Enemy(enemyPreset.NewEnemy()));
                };
            }

        }

        void SpawnBoss()
        {
            if (bossPreset == null) return;
            Addressables.InstantiateAsync(enemyPrefab, bossSpawnPoint, Quaternion.identity).Completed += op =>
            {
                if (op.Status != AsyncOperationStatus.Succeeded) return;
                if (!op.Result.TryGetComponent(out SubRealmEnemy enemyHolder)) return;
                enemyHolder.Setup(new Boss(bossPreset.NewBoss()));
            };
        }

        void OnDrawGizmosSelected()
        {
            foreach (Vector3 spawnPoint in spawnPoints) 
                Gizmos.DrawSphere(spawnPoint, 1f);
            if (bossSpawnPoint != Vector3.zero) 
                Gizmos.DrawSphere(bossSpawnPoint, 2f);
        }
#if UNITY_EDITOR
        public bool AddSpawnPosition(Ray ray)
        {
            NavMeshHit navHit = new();
            bool valid = Physics.Raycast(ray, out RaycastHit hit) &&
                         NavMesh.SamplePosition(hit.point, out navHit, 2f, spawnOn);
            if (valid)
                spawnPoints.Add(navHit.position);
            return valid;
            //&& NotToClose(navHit) && NotToFarAway(navHit);
        }
        public bool AddBossPosition(Ray ray)
        {
            NavMeshHit navHit = new();
            bool valid = Physics.Raycast(ray, out RaycastHit hit) &&
                         NavMesh.SamplePosition(hit.point, out navHit, 2f, spawnOn);
            if (valid)
                bossSpawnPoint = navHit.position;
            return valid;
        }
#endif
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [SerializeField] List<Transform> spawnPoints = new();
        [SerializeField] Transform bossSpawnPoint;

        bool loaded;
        async void Start()
        {
            Task[] tasks = new Task[enemyPresets.Length];
            for(int i = 0; i < enemyPresets.Length; i++) 
                tasks[i] = enemyPresets[i].LoadAssets();
            await Task.WhenAll(tasks);
            if (bossPreset != null) 
                await bossPreset.LoadAssets();
            loaded = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) 
                SpawnEnemies();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) 
                DeSpawnEnemies();
        }

        void DeSpawnEnemies()
        {
            
            
        }

        static System.Random rng = new ();
        void SpawnEnemies()
        {
            if (!loaded)
                StartCoroutine(WaitForLoad());
            SpawnBoss();
            if (enemyPresets.Length <= 0) return;
            foreach (var spawnPoint in spawnPoints)
            {
                Addressables.InstantiateAsync(enemyPrefab, spawnPoint.position, spawnPoint.rotation).Completed += op =>
                {
                    if (op.Status != AsyncOperationStatus.Succeeded) return;
                    if (!op.Result.TryGetComponent(out SubRealmEnemy enemyHolder)) return;
                    var enemyPreset = enemyPresets[rng.Next(0, enemyPresets.Length)];
                    enemyHolder.Setup(new Enemy(enemyPreset.NewEnemy()));
                };
            }

        }

        IEnumerator WaitForLoad()
        {
            yield return new WaitUntil(() => loaded);
            SpawnEnemies();
        }

        void SpawnBoss()
        {
            if (bossPreset == null) return;
            Addressables.InstantiateAsync(enemyPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation).Completed += op =>
            {
                if (op.Status != AsyncOperationStatus.Succeeded) return;
                if (!op.Result.TryGetComponent(out SubRealmEnemy enemyHolder)) return;
                enemyHolder.Setup(new Boss(bossPreset.NewBoss()));
            };
        }

        void OnDrawGizmosSelected()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                Gizmos.DrawSphere(spawnPoint.position, 1f);
                Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + spawnPoint.forward);
            }

            if (bossSpawnPoint == null) return;
            Gizmos.DrawSphere(bossSpawnPoint.position, 2f);
            Gizmos.DrawLine(bossSpawnPoint.position, bossSpawnPoint.position + bossSpawnPoint.forward);
        }
#if UNITY_EDITOR
        public bool AddSpawnPosition(Ray ray)
        {
            NavMeshHit navHit = new();
            bool valid = Physics.Raycast(ray, out var hit) &&
                         NavMesh.SamplePosition(hit.point, out navHit, 2f, spawnOn);
            if (!valid) 
                return false;
            var go = new GameObject("SpawnPoint")
            {
                transform =
                {
                    position = navHit.position,
                },
            };
            go.transform.SetParent(transform);
            spawnPoints.Add(go.transform);
            return true;
            //&& NotToClose(navHit) && NotToFarAway(navHit);
        }
        public bool AddBossPosition(Ray ray)
        {
            NavMeshHit navHit = new();
            bool valid = Physics.Raycast(ray, out RaycastHit hit) &&
                         NavMesh.SamplePosition(hit.point, out navHit, 2f, spawnOn);
            if (!valid) return valid;
            GameObject go = new("BossSpawnPoint");
            go.transform.position = navHit.position;
            go.transform.SetParent(transform);
            if (bossSpawnPoint != null)
                DestroyImmediate(bossSpawnPoint.gameObject);
            bossSpawnPoint = go.transform;
            return valid;
        }
#endif
    }
}
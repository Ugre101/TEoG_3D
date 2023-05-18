using System.Collections;
using System.Collections.Generic;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Holders.SubRealm;
using SaveStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Map.Sub_Realm
{
    public sealed class RealmEnemyZone : MonoBehaviour
    {
        [SerializeField] AssetReference enemyPrefab;
        [SerializeField] EnemyPreset[] enemyPresets;   
        [SerializeField] BossPreset bossPreset;
        [SerializeField] LayerMask spawnOn;
        [SerializeField] List<Transform> spawnPoints = new();
        [SerializeField] Transform bossSpawnPoint;
        [SerializeField] SubRealmSceneSo subRealmSceneSo;

        bool loaded;
        readonly List<SubRealmEnemy> activeEnemies = new();
        async void Start()
        {
            await enemyPresets.LoadEnemyPresets();
            if (bossPreset != null) 
                await bossPreset.LoadAssets();
            loaded = true;
        }

        void OnDestroy()
        {
            foreach (var enemyPreset in enemyPresets) 
                enemyPreset.UnLoad();
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
            foreach (var activeEnemy in activeEnemies) 
                Addressables.ReleaseInstance(activeEnemy.gameObject);
            activeEnemies.Clear();
        }

        static System.Random rng = new ();
        void SpawnEnemies()
        {
            if (!loaded)
                StartCoroutine(WaitForLoad());
            SpawnBoss();
            if (enemyPresets.Length <= 0) return;
            if (subRealmSceneSo.Enemies.TryGetValue(gameObject.name, out var list))
                InstanceSavedEnemies(list);
            else
                InstanceNewEnemies();

        }

        void InstanceNewEnemies()
        {
            subRealmSceneSo.Enemies.TryAdd(gameObject.name, new List<SubRealmSceneSo.SavedEnemy>());
            foreach (var spawnPoint in spawnPoints)
                Addressables.InstantiateAsync(enemyPrefab, spawnPoint.position, spawnPoint.rotation).Completed += op =>
                {
                    if (op.Status != AsyncOperationStatus.Succeeded) return;
                    if (!op.Result.TryGetComponent(out SubRealmEnemy enemyHolder)) return;
                    var enemyPreset = enemyPresets[rng.Next(0, enemyPresets.Length)];
                    enemyHolder.Setup(new Enemy(enemyPreset.NewEnemy()));
                    subRealmSceneSo.Enemies[gameObject.name].Add(new SubRealmSceneSo.SavedEnemy(enemyHolder.Enemy,
                        enemyHolder.transform.position, enemyHolder.transform.rotation));
                    activeEnemies.Add(enemyHolder);
                };
        }

        void InstanceSavedEnemies(List<SubRealmSceneSo.SavedEnemy> list)
        {
            foreach (var savedEnemy in list)
                Addressables.InstantiateAsync(enemyPrefab, savedEnemy.Position, savedEnemy.Rotation).Completed += op =>
                {
                    if (op.Status != AsyncOperationStatus.Succeeded) return;
                    if (!op.Result.TryGetComponent(out SubRealmEnemy enemyHolder)) return;
                    enemyHolder.Setup(savedEnemy.Enemy);
                    activeEnemies.Add(enemyHolder);
                };
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
            if (!valid) return false;
            GameObject go = new("BossSpawnPoint")
            {
                transform =
                {
                    position = navHit.position,
                },
            };
            go.transform.SetParent(transform);
            if (bossSpawnPoint != null)
                DestroyImmediate(bossSpawnPoint.gameObject);
            bossSpawnPoint = go.transform;
            return true;
        }
#endif
    }
}
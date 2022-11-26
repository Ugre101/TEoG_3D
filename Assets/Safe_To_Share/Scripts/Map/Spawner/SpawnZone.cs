using System.Collections.Generic;
using System.Threading.Tasks;
using AvatarStuff.Holders;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Character.IslandData;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Map.Spawner
{
    public class SpawnZone : MonoBehaviour
    {
        const float DontSpawnWithinRangeOfPlayer = 25f;


        static bool alreadyPreloading;
        [SerializeField] EnemyPreset[] enemyPresets;
        [SerializeField] AssetReference prefabRef;
        [SerializeField] int spawnAmount = 10;
        [SerializeField] LayerMask spawnOn = 1;
        [SerializeField] float range = 10f;
        [SerializeField] Islands island;
        [SerializeField, HideInInspector,] int id;
        [SerializeField] List<Vector3> spawnPoints = new();
        bool assetsLoaded;


        bool inRange;

        Transform player;
        Queue<Enemy> returnEnemies;

        async void Start()
        {
            var playerObject = PlayerHolder.Instance;
            if (playerObject == null)
            {
                Debug.LogError("SpawnZone can't find player");
                enabled = false;
                return;
            }

            player = playerObject.transform;

            if (prefabRef != null && !prefabRef.RuntimeKeyIsValid())
                Debug.LogError("prefabRef error");
            if (enemyPresets.Length == 0)
            {
                Debug.LogWarning("Zone is missing enemy presets");
                enabled = false;
                return;
            }

            await PreLoadChars();
        }

        void Update()
        {
            if (!assetsLoaded) return;
            if (!inRange) return;
            if (!FrameLimit()) return;
            if (returnEnemies is { Count: not 0, })
                SpawnAEnemyPrefab(returnEnemies.Dequeue());
        }

        void FixedUpdate()
        {
            if (Time.frameCount % 30 != 0)
                return;
            inRange = WithinRange();
            if (inRange && !alreadyPreloading)
            {
                SceneLoader.Instance.LoadCombatIfNotAlready();
                alreadyPreloading = true;
            }
        }

        void OnDestroy()
        {
            alreadyPreloading = false;
            foreach (EnemyPreset preset in enemyPresets)
                preset.UnLoad();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, range);
            foreach (Vector3 spawnPoint in spawnPoints) Gizmos.DrawSphere(spawnPoint, 1f);
        }

        public void ReSetupQue() => SetupEnemies();

        void SetupEnemies()
        {
            if (!SavedEnemies.Enemies.ContainsKey(id))
                SavedEnemies.SetupZone(id, spawnAmount, island, enemyPresets);
            returnEnemies = new Queue<Enemy>(SavedEnemies.Enemies[id]);
        }

        public void SetId(int value) => id = value;

        static bool FrameLimit() => Time.frameCount % 150 == 0;

        bool WithinRange() =>
            Vector3.Distance(transform.position, player.position) <
            SpawnSettings.SpawnWhenPlayerAreWithinDistance;

        async Task PreLoadChars()
        {
            Task[] tasks = new Task[enemyPresets.Length];
            for (int i = 0; i < enemyPresets.Length; i++)
                tasks[i] = enemyPresets[i].LoadAssets();

            await Task.WhenAll(tasks);

            assetsLoaded = true;
            SetupEnemies();
        }

        void SpawnAEnemyPrefab(Enemy enemy)
        {
            int tries = 0;
            while (!AddEnemy(enemy) && tries < 99) tries++;
        }

        bool AddEnemy(Enemy enemy)
        {
            Vector3 pos = Vector3.zero;
            bool hit = false;
            var tempList = spawnPoints;
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Vector3 spawnPoint = tempList[Random.Range(0, spawnPoints.Count)];
                if (NotToClose(spawnPoint) && NotToFarAway(spawnPoint))
                {
                    pos = spawnPoint;
                    hit = true;
                    break;
                }

                tempList.Remove(spawnPoint);
            }

            if (!hit) return false;
            Addressables.InstantiateAsync(prefabRef.RuntimeKey, pos, Quaternion.identity).Completed +=
                handle => NewPrefab(handle, enemy);
            //   prefabRef.InstantiateAsync(navHit.position, Quaternion.identity).Completed += obj => NewPrefab(obj, enemy);
            return true;
        }

        public bool ValidPosition(Ray ray, out NavMeshHit navHit)
        {
            navHit = new NavMeshHit();
            return Physics.Raycast(ray, out RaycastHit hit) &&
                   NavMesh.SamplePosition(hit.point, out navHit, 2f, spawnOn);
            //&& NotToClose(navHit) && NotToFarAway(navHit);
        }

        public bool AddSpawnPosition(Ray ray)
        {
            NavMeshHit navHit = new();
            bool valid = Physics.Raycast(ray, out RaycastHit hit) &&
                         NavMesh.SamplePosition(hit.point, out navHit, 2f, spawnOn);
            if (valid && Vector3.Distance(transform.position, navHit.position) < range)
                spawnPoints.Add(navHit.position);
            return valid;
            //&& NotToClose(navHit) && NotToFarAway(navHit);
        }

        bool NotToFarAway(Vector3 navHit) => Vector3.Distance(navHit, player.position) <
                                             SpawnSettings.SpawnWhenPlayerAreWithinDistance;

        bool NotToClose(Vector3 navHit) =>
            Vector3.Distance(player.position, navHit) > DontSpawnWithinRangeOfPlayer;

        void NewPrefab(AsyncOperationHandle<GameObject> obj, Enemy enemy)
        {
            if (!obj.Result.TryGetComponent(out EnemyAiHolder holder))
                return;
            holder.AddEnemy(enemy);
            holder.ReturnMe += ReturnOutOfRangeEnemy;
        }

        void ReturnOutOfRangeEnemy(EnemyAiHolder obj)
        {
            returnEnemies.Enqueue(obj.Enemy);
            Addressables.ReleaseInstance(obj.gameObject);
        }

        public void SetIsland(Islands currentIsland) => island = currentIsland;
    }
}
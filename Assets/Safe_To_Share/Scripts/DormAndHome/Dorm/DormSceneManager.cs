using System.Collections.Generic;
using System.Threading.Tasks;
using AvatarStuff.Holders;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DormAndHome.Dorm
{
    public class DormSceneManager : MonoBehaviour
    {
        [SerializeField] float spawnRange;
        [SerializeField] AssetReference prefab;
        [SerializeField] LayerMask spawnOn = 1;

        List<GameObject> spawned = new();

        async void Start()
        {
            DormManager.Loaded += Loaded;
            await SpawnDormMates();
        }


        void OnDestroy() => DormManager.Loaded -= Loaded;

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRange);
        }

        async Task SpawnDormMates()
        {
            int limit = Mathf.Min(DormManager.Instance.DormMates.Count, DormManagerExtensions.FreeRangeLimit);
            Task[] tasks = new Task[limit];
            for (int i = 0; i < limit; i++)
                if (DormManager.Instance.DormMates[i].SleepIn == DormMateSleepIn.Lodge)
                  tasks[i] = SpawnDormMate(DormManager.Instance.DormMates[i]);
            await Task.WhenAll(tasks);
        }

        async Task SpawnDormMate(DormMate instanceDormMate)
        {
            for (int i = 0; i < 99; i++)
            {
                if (await SpawnAMate(instanceDormMate))
                    break;
                if (i == 98)
                    Debug.Log("Wow");
            }
        }

        async Task<bool> SpawnAMate(DormMate instanceDormMate)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRange;
            Ray ray = new(pos + new Vector3(0, 100, 0), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit,200f, spawnOn))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, spawnOn))
                {
                    await LoadPrefab(navHit.position, instanceDormMate);
                    //   Instantiate(prefab, navHit.position, quaternion.identity).AddMate(instanceDormMate);
                    return true;
                }

                return false;
            }

            return false;
        }

        async Task LoadPrefab(Vector3 pos, DormMate instanceDormMate)
        {
            var op = prefab.InstantiateAsync(pos, quaternion.identity).Task;
            await op;
            if (op.IsCompletedSuccessfully)
            {
                spawned.Add(op.Result);
                if (op.Result.TryGetComponent(out DormMateAiHolder holder)) 
                    holder.AddMate(instanceDormMate);
            }
        }


        public async void Loaded()
        {
            foreach (GameObject o in spawned)
                Addressables.ReleaseInstance(o);
            spawned = new List<GameObject>();
            await SpawnDormMates();
        }
    }
}
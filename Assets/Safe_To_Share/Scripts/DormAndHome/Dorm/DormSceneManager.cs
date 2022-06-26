using AvatarStuff.Holders;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DormAndHome.Dorm
{
    public class DormSceneManager : MonoBehaviour
    {
        [SerializeField] float spawnRange;
        [SerializeField] DormMateAiHolder prefab;
        [SerializeField] LayerMask spawnOn = 1;

        void Start()
        {
            DormManager.Loaded += Loaded;
            SpawnDormMates();
        }

        void OnDestroy() => DormManager.Loaded -= Loaded;

        void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, spawnRange);

        void SpawnDormMates()
        {
            for (int i = 0; i < DormManager.Instance.DormMates.Count && i < DormManagerExtensions.FreeRangeLimit; i++)
                if (DormManager.Instance.DormMates[i].SleepIn == DormMateSleepIn.Lodge)
                    SpawnDormMate(DormManager.Instance.DormMates[i]);
        }

        void SpawnDormMate(DormMate instanceDormMate)
        {
            for (int i = 0; i < 99; i++)
                if (SpawnAMate(instanceDormMate))
                    break;
        }

        bool SpawnAMate(DormMate instanceDormMate)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRange;
            Ray ray = new(pos + new Vector3(0, 10, 0), Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hit, spawnOn) ||
                !NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, spawnOn)) return false;
            Instantiate(prefab, navHit.position, quaternion.identity, transform).AddMate(instanceDormMate);
            return true;
        }

        public void Loaded()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            SpawnDormMates();
        }
    }
}
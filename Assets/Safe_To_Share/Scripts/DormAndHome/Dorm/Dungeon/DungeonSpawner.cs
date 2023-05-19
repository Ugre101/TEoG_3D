using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DormAndHome.Dorm.Dungeon
{
    public sealed class DungeonSpawner : MonoBehaviour
    {
        [SerializeField] List<DungeonSpawnSpot> spawnSpots = new();

        readonly WaitForSeconds waitForSeconds = new(2f);

        IEnumerator Start()
        {
            yield return waitForSeconds;
            DormManager.Loaded += Loaded;
            SpawnDungeonMates();
        }


        void OnDestroy() => DormManager.Loaded -= Loaded;
#if UNITY_EDITOR

        void OnValidate()
        {
            if (Application.isPlaying)
                return;
            spawnSpots = new List<DungeonSpawnSpot>(GetComponentsInChildren<DungeonSpawnSpot>());
        }
#endif

        void SpawnDungeonMates()
        {
            for (int i = 0; i < DormManager.Instance.DormMates.Count && i < spawnSpots.Count; i++)
                if (DormManager.Instance.DormMates[i].SleepIn == DormMateSleepIn.Dungeon)
                    SpawnDungeonMate(DormManager.Instance.DormMates[i]);
        }

        void SpawnDungeonMate(DormMate instanceDormMate)
        {
            foreach (DungeonSpawnSpot spawnSpot in spawnSpots.Where(spawnSpot => spawnSpot.Empty))
            {
                spawnSpot.Setup(instanceDormMate);
                break;
            }
        }

        void Loaded()
        {
            foreach (DungeonSpawnSpot spawnSpot in spawnSpots)
                spawnSpot.Clear();
            SpawnDungeonMates();
        }
    }
}
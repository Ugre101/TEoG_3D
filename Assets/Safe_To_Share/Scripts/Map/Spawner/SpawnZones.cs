using Character.IslandData;
using Unity.Mathematics;
using UnityEngine;

namespace Map.Spawner
{
    public sealed class SpawnZones : MonoBehaviour
    {
        [SerializeField] SpawnZone zone;
        [SerializeField] SpawnZone[] zones = { };
        [SerializeField] Islands island;
        public static SpawnZones Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Debug.LogWarning("Duplicate spawnzones main");
        }
#if UNITY_EDITOR
        void OnValidate()
        {
            zones = gameObject.GetComponentsInChildren<SpawnZone>();
            foreach (var z in zones)
                z.SetIsland(island);
        }
#endif
        public void AddNewZone(Vector3 pos)
        {
            SpawnZone newZone = Instantiate(zone, pos, quaternion.identity, transform);
        }

        public void ClearEnemies()
        {
            foreach (var z in zones)
                z.ReSetupQue();
        }
    }
}
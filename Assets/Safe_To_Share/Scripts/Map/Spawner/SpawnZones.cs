using Character.IslandData;
using Unity.Mathematics;
using UnityEngine;

namespace Map.Spawner
{
    public class SpawnZones : MonoBehaviour
    {
        public static SpawnZones Instance { get; private set; }
        [SerializeField] SpawnZone zone;
        [SerializeField] SpawnZone[] zones = { };
        [SerializeField] Islands island;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Debug.LogWarning("Duplicate spawnzones main");
        }
        public void AddNewZone(Vector3 pos)
        {
            SpawnZone newZone = Instantiate(zone, pos, quaternion.identity, transform);
        }
        public void ClearEnemies()
        {
            foreach (var z in zones)
                z.ReSetupQue();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            zones =  gameObject.GetComponentsInChildren<SpawnZone>();
            foreach (var z in zones)
                z.SetIsland(island);
        }
#endif
    }
}
using System;
using Map;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    public class MapData : MonoBehaviour
    {
        public static MapData Instance { get; private set; }
        [field: SerializeField] public StaticMiniMapObject[] Statics { get; private set; }
        [field: SerializeField] public EnemyZoneMiniMapObject[] enemyZones { get; private set; }
        [field: SerializeField] public UnLockableMiniMapObject[] UnLockable { get; private set; } = Array.Empty<UnLockableMiniMapObject>();
        [field: SerializeField] public Vector2 MapSize { get; private set; }
        [field: SerializeField] public Vector3 MapPosition { get; private set; }
        void Awake() => Instance = this;

        [ContextMenu("Editor Setup")]
        void EditorSetup()
        {
            Statics = FindObjectsOfType<StaticMiniMapObject>(true);
            enemyZones = FindObjectsOfType<EnemyZoneMiniMapObject>(true);
            UnLockable = FindObjectsOfType<UnLockableMiniMapObject>(true);
            if (gameObject.TryGetComponent(out Terrain terrain))
            {
                MapSize = new Vector2(terrain.terrainData.size.x, terrain.terrainData.size.z);
                MapPosition = terrain.GetPosition();
            }
            else if (MapSize == Vector2.zero)
            {
                Debug.Log("Map data isn't attached to a terrain, attach it or manually enter map size and map zero pos");
            }
        }
    }
}
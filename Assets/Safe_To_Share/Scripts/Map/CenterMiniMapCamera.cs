using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    public sealed class CenterMiniMapCamera : MonoBehaviour {
        [SerializeField] float yOffset;

        void OnValidate() => Center();

        // Start is called before the first frame update
        void Center() {
            if (Terrain.activeTerrain == null) return;
            var terrainDataSize = Terrain.activeTerrain.terrainData.size;
            var xMid = terrainDataSize.x / 2;
            var zMid = terrainDataSize.z / 2;
            var biggest = Mathf.Max(xMid, zMid);
            var terrainPos = Terrain.activeTerrain.GetPosition();
            transform.position = new Vector3(terrainPos.x + xMid, biggest, terrainPos.z + zMid);
        }
    }
}
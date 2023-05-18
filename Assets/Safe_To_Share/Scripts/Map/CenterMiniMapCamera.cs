using UnityEngine;

namespace Map
{
    public sealed class CenterMiniMapCamera : MonoBehaviour
    {
        [SerializeField] float yOffset;

        void OnValidate() => Center();

        // Start is called before the first frame update
        void Center()
        {
            if (Terrain.activeTerrain == null) return;
            Vector3 terrainDataSize = Terrain.activeTerrain.terrainData.size;
            float xMid = terrainDataSize.x / 2;
            float zMid = terrainDataSize.z / 2;
            float biggest = Mathf.Max(xMid, zMid);
            Vector3 terrainPos = Terrain.activeTerrain.GetPosition();
            transform.position = new Vector3(terrainPos.x + xMid, biggest, terrainPos.z + zMid);
        }
    }
}
using UnityEngine;

namespace Safe_To_Share.Scripts {
    public sealed class HideTerrainTiles : MonoBehaviour {
        [SerializeField, Range(1, 5),] int cullingRange = 1;

        Vector3 lastPos;

        Vector3 offset;
        GameObject[] terrainTiles;

        void Start() {
            terrainTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
            if (!terrainTiles[0].TryGetComponent(out Terrain terrain)) return;
            var terrainData = terrain.terrainData;
            offset = terrainData.size / 2;
        }

        // Update is called once per frame
        void Update() {
            if (lastPos == transform.position)
                return;
            lastPos = transform.position;
            foreach (var terrainTile in terrainTiles) {
                var tilePosition = terrainTile.transform.position + offset;

                var xDistance = Mathf.Abs(tilePosition.x - lastPos.x);
                var zDistance = Mathf.Abs(tilePosition.z - lastPos.z);
                terrainTile.SetActive(!(xDistance + zDistance > offset.x * cullingRange));
            }
        }
    }
}
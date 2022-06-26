using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class HideTerrainTiles : MonoBehaviour
    {
        GameObject[] terrainTiles;

        [SerializeField,Range(1,5),] int cullingRange = 1;

        Vector3 offset;
        void Start()
        {
            terrainTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
            if(terrainTiles[0].TryGetComponent(out  Terrain terrain))
            {
                var terrainData = terrain.terrainData;
                offset = terrainData.size / 2;
            }
        }

        Vector3 lastPos;
        // Update is called once per frame
        void Update()
        {
            if (lastPos == transform.position) 
                return;
            lastPos = transform.position;
            foreach (GameObject terrainTile in terrainTiles)
            {
                Vector3 tilePosition = terrainTile.transform.position + offset;

                float xDistance = Mathf.Abs(tilePosition.x - lastPos.x);
                float zDistance = Mathf.Abs(tilePosition.z - lastPos.z);
                terrainTile.SetActive(!(xDistance + zDistance > offset.x * cullingRange));
            }
        }
    }
}

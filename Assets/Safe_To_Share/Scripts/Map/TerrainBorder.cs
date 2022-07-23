using UnityEngine;

namespace Map
{
    public class TerrainBorder : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] float borderHeight = 50f;
        [SerializeField] float moveBorderIn = 2f;
        [SerializeField] Transform north, south, east, west;
        void OnValidate() => BuildBorder();

        void BuildBorder()
        {
            Terrain active = Terrain.activeTerrain;
            if (active == null) return;
            TerrainData data = active.terrainData;
            transform.localPosition = data.bounds.center;
            north.localPosition = new Vector3(data.bounds.center.x - moveBorderIn, 0, 0);
            south.localPosition = new Vector3(-data.bounds.center.x + moveBorderIn, 0, 0);
            east.localPosition = new Vector3(0, 0, data.bounds.center.z - moveBorderIn);
            west.localPosition = new Vector3(0, 0, -data.bounds.center.z + moveBorderIn);
            SetScales(data);
        }

        void SetScales(TerrainData data)
        {
            Vector3 zScaleWall = new(0, borderHeight, data.bounds.size.z);
            Vector3 xScaleWall = new(data.bounds.size.x, borderHeight, 0);
            north.localScale = zScaleWall;
            south.localScale = zScaleWall;
            east.localScale = xScaleWall;
            west.localScale = xScaleWall;
        }
#endif
    }
}
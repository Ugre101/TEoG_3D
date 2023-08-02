using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    public sealed class TerrainBorder : MonoBehaviour {
#if UNITY_EDITOR
        [SerializeField] float borderHeight = 50f;
        [SerializeField] float moveBorderIn = 2f;
        [SerializeField] Transform north, south, east, west;
        void OnValidate() => BuildBorder();

        void BuildBorder() {
            var active = Terrain.activeTerrain;
            if (active == null) return;
            var data = active.terrainData;
            transform.localPosition = data.bounds.center;
            (north != null ? north : CreateCube(out north)).localPosition =
                new Vector3(data.bounds.center.x - moveBorderIn, 0, 0);
            (south != null ? south : CreateCube(out south)).localPosition =
                new Vector3(-data.bounds.center.x + moveBorderIn, 0, 0);
            (east != null ? east : CreateCube(out east)).localPosition =
                new Vector3(0, 0, data.bounds.center.z - moveBorderIn);
            (west != null ? west : CreateCube(out west)).localPosition =
                new Vector3(0, 0, -data.bounds.center.z + moveBorderIn);
            SetScales(data);
        }

        Transform CreateCube(out Transform borderSide) {
            var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            primitive.transform.SetParent(transform);
            primitive.name = nameof(borderSide);
            borderSide = primitive.transform;
            return primitive.transform;
        }

        void SetScales(TerrainData data) {
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
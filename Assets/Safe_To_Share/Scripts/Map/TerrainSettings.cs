using System;
using CustomClasses;
using UnityEngine;

namespace Map
{
    public class TerrainSettings : MonoBehaviour
    {
        const string TerrainDetailSave = "TerrainDetailDensity";
        public static readonly SavedFloatSetting TerrainDetail = new(TerrainDetailSave, 0.5f);

        void Start()
        {
            if (Terrain.activeTerrains == null) return;
            foreach (Terrain activeTerrain in Terrain.activeTerrains)
                if (Math.Abs(activeTerrain.detailObjectDensity - TerrainDetail.Value) > 0.01f)
                    activeTerrain.detailObjectDensity = TerrainDetail.Value;
        }
    }
}
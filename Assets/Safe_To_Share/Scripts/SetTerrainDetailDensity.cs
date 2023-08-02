using Safe_To_Share.Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts {
    public sealed class SetTerrainDetailDensity : MonoBehaviour {
        [SerializeField] Slider slider;

        [SerializeField] TextMeshProUGUI text;

        void Start() {
            UpdateText(TerrainSettings.TerrainDetail.Value);
            slider.SetValueWithoutNotify(TerrainSettings.TerrainDetail.Value);
        }

        void UpdateText(float dist) => text.text = $"{dist:0.##}";

        public void ChangeRenderDist(float arg0) {
            arg0 = Mathf.Clamp(arg0, 0f, 1f);
            TerrainSettings.TerrainDetail.Value = arg0;
            foreach (var activeTerrain in Terrain.activeTerrains)
                activeTerrain.detailObjectDensity = arg0;
            UpdateText(arg0);
        }
    }
}
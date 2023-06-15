using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Options {
    public sealed class ToggleShadows : MonoBehaviour {
        [SerializeField] RenderPipelineAsset noShadow;
        [SerializeField] RenderPipelineAsset withShadow;

        // Start is called before the first frame update
        void Start() {
            if (TryGetComponent(out Toggle toggle)) toggle.onValueChanged.AddListener(Change);
        }

        void Change(bool arg0) => QualitySettings.renderPipeline = arg0 ? withShadow : noShadow;
    }
}
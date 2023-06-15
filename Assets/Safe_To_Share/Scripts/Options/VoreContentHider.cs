using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Options {
    public sealed class VoreContentHider : MonoBehaviour {
        void OnEnable() {
            if (!OptionalContent.Vore.Enabled)
                gameObject.SetActive(false);
        }
    }
}
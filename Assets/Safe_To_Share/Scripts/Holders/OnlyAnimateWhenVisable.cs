using UnityEngine;

namespace Safe_To_Share.Scripts.Holders {
    public sealed class OnlyAnimateWhenVisable : MonoBehaviour {
        const float CheckEvery = 2f;
        [SerializeField] Renderer[] renderers;
        [SerializeField] Animator animator;
        float lastTick = 999f;
        void Update() {
            if (!(Time.time < lastTick + CheckEvery)) return;
            lastTick = Time.time;
            var visible = false;
            foreach (var rend in renderers)
                if (rend.isVisible)
                    visible = true;
            animator.enabled = visible;
        }
#if UNITY_EDITOR

        void OnValidate() {
            if (Application.isPlaying) return;
            animator = GetComponent<Animator>();
            renderers = GetComponentsInChildren<Renderer>();
        }
#endif
    }
}
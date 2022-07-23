using UnityEngine;

namespace Holders
{
    public class OnlyAnimateWhenVisable : MonoBehaviour
    {
        const float CheckEvery = 2f;
        [SerializeField] Renderer[] renderers;
        [SerializeField] Animator animator;
        float lastTick = 999f;
        void Update()
        {
            if (Time.time < lastTick + CheckEvery)
            {
                lastTick = Time.time;
                bool visible = false;
                foreach (Renderer rend in renderers)
                    if (rend.isVisible)
                        visible = true;
                animator.enabled = visible;
            }
        }
#if UNITY_EDITOR

        void OnValidate()
        {
            if (Application.isPlaying) return;
            animator = GetComponent<Animator>();
            renderers = GetComponentsInChildren<Renderer>();
        }
#endif
    }
}
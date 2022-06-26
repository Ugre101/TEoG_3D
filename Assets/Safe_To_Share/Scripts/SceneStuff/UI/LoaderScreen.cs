using System.Collections;
using TMPro;
using UnityEngine;

namespace SceneStuff
{
    public class LoaderScreen : MonoBehaviour
    {
        private const float Interval = 0.192f;
        [SerializeField] TextMeshProUGUI progressText;
        [SerializeField] CanvasGroup group;
        [SerializeField, Range(0.2f, 0.8f)] float aplhaCutOff = 0.2f;
        public void LoadProgress(float value) => progressText.text = $"Loading... {value}";

        public void UnLoadProgress(float value) => progressText.text = $"Unloading... {value}";

        Coroutine running;

        [ContextMenu("Fade In")]
        public void StopFade()
        {
            if (running != null)
                StopCoroutine(running);
            running = StartCoroutine(FadeIn());
        }
        [ContextMenu("Fade out")]
        public void StartFade()
        {
            if (running != null)
                StopCoroutine(running);
            running = StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            float startTime = Time.realtimeSinceStartup;
            group.blocksRaycasts = true;
            while (group.alpha < 1f)
            {
                group.alpha = Mathf.SmoothStep(0, 1, TimeStep(startTime));
                yield return null;
            }
        }
        private IEnumerator FadeIn()
        {
            float startTime = Time.realtimeSinceStartup;
            while (group.alpha > aplhaCutOff)
            {
                group.alpha = Mathf.SmoothStep(1, aplhaCutOff, TimeStep(startTime));
                // group.alpha = Mathf.Max(group.alpha - intrevall * Time.deltaTime, 0f);
                yield return null;
            }
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }

        private static float TimeStep(float startTime) => (Time.realtimeSinceStartup - startTime) * 4f;
    }
}
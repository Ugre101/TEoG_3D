using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public sealed class SleepCanvas : MonoBehaviour
    {
        [SerializeField] GameCanvas gameCanvas;
        [SerializeField] CanvasGroup canvasGroup;
        readonly WaitForSecondsRealtime waitForSecondsRealtime = new(0.2f);
        Coroutine coroutine;

        public void Sleep()
        {
            gameObject.SetActive(true);
            coroutine = StartCoroutine(FadeInAndOut());
        }

        void OnDisable()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }


        IEnumerator FadeInAndOut()
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += 0.01f;
                yield return null;
            }
            yield return waitForSecondsRealtime;
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= 0.01f;
                yield return null;
            }
            gameObject.SetActive(false);
            gameCanvas.CloseMenus();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using GameUIAndMenus;
using UnityEngine;

public class SleepCanvas : MonoBehaviour
{
    [SerializeField] GameCanvas gameCanvas;
    [SerializeField] CanvasGroup canvasGroup;
    readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    readonly WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.2f);
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
            yield return waitForEndOfFrame;
        }
        yield return waitForSecondsRealtime;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.01f;
            yield return waitForEndOfFrame;
        }
        gameObject.SetActive(false);
        gameCanvas.CloseMenus();
    }
}

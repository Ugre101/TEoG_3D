using System.Collections;
using System.Collections.Generic;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Optimazation
{
    public sealed class DelayedStartManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> startLates;
#if UNITY_EDITOR
        [SerializeField] bool editorWaitForColdStart;
#endif
        readonly WaitForEndOfFrame waitForEndOfFrame = new();
        readonly WaitForSecondsRealtime waitForSecondsRealtime = new(0.2f);

        IEnumerator Start()
        {
#if UNITY_EDITOR
            if (editorWaitForColdStart && SceneLoader.CurrentLocation == null)
            {
                yield break;
            }
#endif
            yield return waitForSecondsRealtime;
            foreach (var startLate in startLates)
            {
                yield return waitForEndOfFrame;
                startLate.SetActive(true);
            }
        }
    }
}
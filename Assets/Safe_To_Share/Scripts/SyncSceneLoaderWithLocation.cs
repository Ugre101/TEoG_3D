#if UNITY_EDITOR
using System.Collections;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class SyncSceneLoaderWithLocation : MonoBehaviour
    {
        [SerializeField] LocationSceneSo locationSceneSo;
        readonly WaitForSeconds coldStartAfterDelay = new(1f);

        IEnumerator Start()
        {
            if (!GameTester.GetFirstCall()) 
                yield break;
            if (locationSceneSo == null)
                Debug.LogError("You forgot to add sceneData to editorTools, saving won't work in editor now.");
            yield return coldStartAfterDelay;
            SceneLoader.Instance.EditorColdStart(locationSceneSo);
        }
    }
}
#endif

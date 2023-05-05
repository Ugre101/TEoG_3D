using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneStuff
{
    [Serializable]
    public class FreePlayUILoader
    {
        [SerializeField] SceneUISo gameUI;

        public IEnumerator LoadGameUI()
        {
            yield return null;
            yield return gameUI.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        }

        public async Task LoadGameUIAsync() => await gameUI.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).Task;

        public AsyncOperationHandle<SceneInstance> UnLoadUI() => gameUI.SceneReference.UnLoadScene();
    }
}
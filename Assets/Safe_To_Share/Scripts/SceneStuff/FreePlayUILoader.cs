using System;
using System.Collections;
using System.Threading.Tasks;
using SaveStuff;
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
        readonly WaitForEndOfFrame waitAFrame = new();

        public IEnumerator LoadGameUI()
        {
            yield return waitAFrame;
            yield return gameUI.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        }  
        public async Task LoadGameUIAsync()
        {
            await gameUI.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).Task;
        }

        public AsyncOperationHandle<SceneInstance> UnLoadUI()
        {
            return gameUI.SceneReference.UnLoadScene();
        }
    }
}
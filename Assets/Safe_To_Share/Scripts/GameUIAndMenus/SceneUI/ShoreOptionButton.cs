using System;
using System.Collections;
using Map;
using SceneStuff;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.SceneUI
{
    public sealed class ShoreOptionButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI descText;
        [SerializeField] Button[] exitBtns;

        LocationSceneSo gameSceneSo;
        public static event Action<LocationSceneSo, SceneTeleportExit> LoadScene;

        public void Setup(KnowLocationsManager.Location location) => StartCoroutine(LoadAssets(location));

        IEnumerator LoadAssets(KnowLocationsManager.Location location)
        {
            var locOp = Addressables.LoadAssetAsync<LocationSceneSo>(location.LocationGuid);
            yield return locOp;
            if (locOp.Status == AsyncOperationStatus.Succeeded)
            {
                gameSceneSo = locOp.Result;
                titleText.text = gameSceneSo.SceneName;
                descText.text = gameSceneSo.description;
            }

            for (int index = 0; index < location.ExitsGuids.Count; index++)
                yield return SetupExitButton(location.ExitsGuids[index], exitBtns[index]);
        }

        IEnumerator SetupExitButton(string v, Button button)
        {
            var exitOp = Addressables.LoadAssetAsync<SceneTeleportExit>(v);
            yield return exitOp;
            if (exitOp.Status != AsyncOperationStatus.Succeeded)
                yield break;
            button.gameObject.SetActive(true);
            button.onClick.AddListener(GoToExit);
            void GoToExit() => LoadScene?.Invoke(gameSceneSo, exitOp.Result);
        }
    }
}
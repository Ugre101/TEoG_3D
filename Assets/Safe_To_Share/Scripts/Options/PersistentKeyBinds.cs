using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.Options {
    public sealed class PersistentKeyBinds : MonoBehaviour {
        const string FreePlaySave = "FreePlayKeyBindsSave";
        const string ThirdPersonSave = "ThirdPersonKeyBindsSave";
        const string GameMenusSave = "GameMenuKeyBindsSave";
        const string QuickSaveSave = "quickSaveKeyBindsSave";
        [SerializeField] InputActionAsset freePlay;
        [SerializeField] InputActionAsset thirdPersonCamera;
        [SerializeField] InputActionAsset gameMenusBinds;
        [SerializeField] InputActionAsset quickSaveBinds;

        IEnumerator Start() {
            OverWriteIfNotNull(freePlay, FreePlaySave);
            yield return null;
            OverWriteIfNotNull(thirdPersonCamera, ThirdPersonSave);
            yield return null;
            OverWriteIfNotNull(gameMenusBinds, GameMenusSave);
            yield return null;
            OverWriteIfNotNull(quickSaveBinds, QuickSaveSave);
        }

        void OnApplicationQuit() {
            PlayerPrefs.SetString(FreePlaySave, freePlay.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(ThirdPersonSave, thirdPersonCamera.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(GameMenusSave, gameMenusBinds.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(QuickSaveSave, quickSaveBinds.SaveBindingOverridesAsJson());
        }

        static void OverWriteIfNotNull(InputActionAsset toRebind, string playerPrefPath) {
            if (!PlayerPrefs.HasKey(playerPrefPath)) return;
            var save = PlayerPrefs.GetString(playerPrefPath, string.Empty);
            if (!string.IsNullOrWhiteSpace(save))
                toRebind.LoadBindingOverridesFromJson(save);
        }
    }
}
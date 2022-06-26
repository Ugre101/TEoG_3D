using UnityEngine;
using UnityEngine.InputSystem;

namespace Options
{
    public class PersistentKeyBinds : MonoBehaviour
    {
        [SerializeField] InputActionAsset freePlay;
        [SerializeField] InputActionAsset thirdPersonCamera;
        [SerializeField] InputActionAsset gameMenusBinds;
        [SerializeField] InputActionAsset quickSaveBinds;
        const string FreePlaySave = "FreePlayKeyBindsSave";
        const string ThirdPersonSave = "ThirdPersonKeyBindsSave";
        const string GameMenusSave = "GameMenuKeyBindsSave";
        const string QuickSaveSave = "quickSaveKeyBindsSave";

        private void Start()
        {
            OverWriteIfNotNull(freePlay, FreePlaySave);
            OverWriteIfNotNull(thirdPersonCamera, ThirdPersonSave);
            OverWriteIfNotNull(gameMenusBinds, GameMenusSave);
            OverWriteIfNotNull(quickSaveBinds, QuickSaveSave);
        }

        static void OverWriteIfNotNull(InputActionAsset toRebind,string playerPrefPath)
        {
            string save = PlayerPrefs.GetString(playerPrefPath, string.Empty);
            if (!string.IsNullOrWhiteSpace(save))
                toRebind.LoadBindingOverridesFromJson(save);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString(FreePlaySave, freePlay.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(ThirdPersonSave, thirdPersonCamera.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(GameMenusSave, gameMenusBinds.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(QuickSaveSave, quickSaveBinds.SaveBindingOverridesAsJson());
        }

    }
}
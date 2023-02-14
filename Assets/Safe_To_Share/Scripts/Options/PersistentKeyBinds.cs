using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Options
{
    public class PersistentKeyBinds : MonoBehaviour
    {
        const string FreePlaySave = "FreePlayKeyBindsSave";
        const string ThirdPersonSave = "ThirdPersonKeyBindsSave";
        const string GameMenusSave = "GameMenuKeyBindsSave";
        const string QuickSaveSave = "quickSaveKeyBindsSave";
        [SerializeField] InputActionAsset freePlay;
        [SerializeField] InputActionAsset thirdPersonCamera;
        [SerializeField] InputActionAsset gameMenusBinds;
        [SerializeField] InputActionAsset quickSaveBinds;

        IEnumerator Start()
        {
            OverWriteIfNotNull(freePlay, FreePlaySave);
            yield return new WaitForEndOfFrame();
            OverWriteIfNotNull(thirdPersonCamera, ThirdPersonSave);
            yield return new WaitForEndOfFrame();
            OverWriteIfNotNull(gameMenusBinds, GameMenusSave);
            yield return new WaitForEndOfFrame();
            OverWriteIfNotNull(quickSaveBinds, QuickSaveSave);
        }

        void OnApplicationQuit()
        {
            PlayerPrefs.SetString(FreePlaySave, freePlay.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(ThirdPersonSave, thirdPersonCamera.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(GameMenusSave, gameMenusBinds.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(QuickSaveSave, quickSaveBinds.SaveBindingOverridesAsJson());
        }

        static void OverWriteIfNotNull(InputActionAsset toRebind, string playerPrefPath)
        {
            if (!PlayerPrefs.HasKey(playerPrefPath)) return;
            string save = PlayerPrefs.GetString(playerPrefPath, string.Empty);
            if (!string.IsNullOrWhiteSpace(save))
            {
                toRebind.LoadBindingOverridesFromJson(save);
            }
        }
        
        
    }
}
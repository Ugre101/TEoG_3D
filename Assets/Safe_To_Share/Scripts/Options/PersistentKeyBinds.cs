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

        void Start()
        {
            OverWriteIfNotNull(freePlay, FreePlaySave);
            OverWriteIfNotNull(thirdPersonCamera, ThirdPersonSave);
            OverWriteIfNotNull(gameMenusBinds, GameMenusSave);
            OverWriteIfNotNull(quickSaveBinds, QuickSaveSave);
        }

        void OnApplicationQuit()
        {
            PlayerPrefs.SetString(FreePlaySave, freePlay.ToJson());
            PlayerPrefs.SetString(ThirdPersonSave, thirdPersonCamera.ToJson());
            PlayerPrefs.SetString(GameMenusSave, gameMenusBinds.ToJson());
            PlayerPrefs.SetString(QuickSaveSave, quickSaveBinds.ToJson());
        }

        static void OverWriteIfNotNull(InputActionAsset toRebind, string playerPrefPath)
        {
            if (!PlayerPrefs.HasKey(playerPrefPath)) return;
            string save = PlayerPrefs.GetString(playerPrefPath, string.Empty);
            if (!string.IsNullOrWhiteSpace(save))
                toRebind.LoadFromJson(save);
        }
    }
}
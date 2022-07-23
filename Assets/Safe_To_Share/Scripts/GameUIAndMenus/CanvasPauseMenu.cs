using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomClasses;
using Options;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameUIAndMenus
{
    public class CanvasPauseMenu : MonoBehaviour
    {
        public static List<ICancelMeBeforeOpenPauseMenu> CancelMe = new();
        static bool sceneDirty = true;
        [SerializeField] InputAction bindings;
        [SerializeField] GameObject mainMenu, settingMenu;
        [SerializeField] Button pauseBtn;
        [SerializeField] LoadMasterAudio masterAudio;
        readonly WaitForEndOfFrame waitForEndOfFrame = new();
        IEnumerable<ICancelMeBeforeOpenPauseMenu> cancelThese;

        void Start()
        {
            bindings.performed += ctx => TogglePause();
            SceneLoader.NewScene += SetDirty;
            LoadSubSceneWhenClose.SubSceneChange += SetDirty;
            // Load Audio settings
            LoadSettings();
        }

        void OnEnable() => bindings.Enable();

        void OnDisable() => bindings.Disable();

        void OnDestroy()
        {
            bindings.Dispose();
            SceneLoader.NewScene -= SetDirty;
            LoadSubSceneWhenClose.SubSceneChange -= SetDirty;
        }

        void LoadSettings()
        {
            VSyncToggle.LoadSetting();
            StartCoroutine(DelayedLoadAudio());
        }

        IEnumerator DelayedLoadAudio()
        {
            yield return waitForEndOfFrame;
            masterAudio.Setup();
            GlobalVolumeControll.LoadGlobalVolume();
        }

        void TogglePause()
        {
            if (mainMenu.activeSelf || settingMenu.activeSelf)
                Resume();
            else
            {
                if (sceneDirty)
                {
                    cancelThese = FindObjectsOfType<MonoBehaviour>(true).OfType<ICancelMeBeforeOpenPauseMenu>();
                    sceneDirty = false;
                }

                if (cancelThese.Any(cancelMeFirst => cancelMeFirst.BlockIfActive()))
                    return;
                if (CancelMe.Any(c => c.BlockIfActive()))
                    return;

                Pause();
            }
        }

        public static void SetDirty() => sceneDirty = true;

        public void Pause()
        {
            SetActive(true);
            GameManager.Pause();
        }

        public void Resume()
        {
            SetActive(false);
            GameManager.Resume(false);
        }

        void SetActive(bool paused)
        {
            mainMenu.SetActive(paused);
            pauseBtn.gameObject.SetActive(!paused);
            settingMenu.SetActive(false);
        }


        public void Exit() => Application.Quit();
    }
}
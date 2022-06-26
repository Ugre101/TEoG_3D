using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Static;
using CustomClasses;
using Options;
using SceneStuff;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameUIAndMenus
{
    public class CanvasPauseMenu : MonoBehaviour
    {
        public static List<ICancelMeBeforeOpenPauseMenu> CancelMe = new();
        [SerializeField] InputAction bindings;
        [SerializeField] GameObject mainMenu, settingMenu;
        [SerializeField] Button pauseBtn;
        [SerializeField] LoadMasterAudio masterAudio;
        readonly WaitForEndOfFrame waitForEndOfFrame = new();
        IEnumerable<ICancelMeBeforeOpenPauseMenu> cancelThese;
        bool sceneDirty = true;

        void Start()
        {
            bindings.performed += ctx => TogglePause();
            SceneLoader.NewScene += SetDirty;
            // Load Audio settings
            LoadSettings();
        }

        void OnEnable() => bindings.Enable();

        void OnDisable() => bindings.Disable();

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

        void SetDirty() => sceneDirty = true;

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
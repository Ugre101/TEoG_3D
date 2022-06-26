using System.IO;
using System.Linq;
using CustomClasses;
using SaveStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.StartScene
{
    public class StartCanvas : MonoBehaviour,ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] GameObject startMenu;
        [SerializeField] SetupPlayer setupPlayer;
        [SerializeField] SaveButton continueLastGame;
        // Start is called before the first frame update
        void Start()
        {
            startMenu.SetActive(false);
            Addressables.InitializeAsync().Completed += AssetsLoaded;
            setupPlayer.LoadAssets();
        }
        

        void AssetsLoaded(AsyncOperationHandle<IResourceLocator> obj)
        {
            startMenu.SetActive(true);
            LoadLastGame();
        }

        public void ReturnToStartMenu() => transform.SleepChildren(startMenu);

        public void QuitGame() => Application.Quit();
        public bool BlockIfActive() => true; // Always block

        void LoadLastGame()
        {
            IOrderedEnumerable<string> saves
                = Directory.GetFiles(SaveManager.SavePath).OrderByDescending(Directory.GetLastWriteTime);
            string savePath = saves.FirstOrDefault();
            if (string.IsNullOrEmpty(savePath))
                continueLastGame.gameObject.SetActive(false);
            else
            {
                try
                {
                    FullSave fullSave = JsonUtility.FromJson<FullSave>(File.ReadAllText(savePath));
                    continueLastGame.Setup(fullSave, savePath);
                    continueLastGame.gameObject.SetActive(true);
                }
                catch
                {
                    continueLastGame.gameObject.SetActive(false);
                }
            }
        }
    }
}
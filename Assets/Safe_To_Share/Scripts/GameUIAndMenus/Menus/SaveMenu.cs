using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SaveStuff;
using UnityEngine;

namespace GameUIAndMenus.Menus
{
    public class SaveMenu : GameMenu
    {
      
        static string[] saves;
        [SerializeField] Transform content;
        [SerializeField] SaveButton saveBtn;
        [SerializeField] AreYouSure areYouSure;
        [SerializeField] ShowSavesAmountSlider showSavesAmount;
        [SerializeField] SaveButton[] preInstanced;

        Queue<SaveButton> ButtonPool;

        List<SaveButton> activeButtons = new();
        SaveButton GetButton()
        {
            if (ButtonPool.Count <= 0) return Instantiate(saveBtn, content);
            var btn = ButtonPool.Dequeue();
            btn.gameObject.SetActive(true);
            return btn;
        }
        bool firstStart = true;
        int failed;
        void Start()
        {
            firstStart = false;
            ButtonPool = new Queue<SaveButton>(preInstanced);
            RefreshSaves();
            showSavesAmount.SetupSlider(saves.Count());
        }

        void OnEnable()
        {
            areYouSure.gameObject.SetActive(false);
            if (!firstStart && SaveManager.SavesDirty)
                RefreshSaves();
            SaveButton.WantToDeleteSave += ToggleAreYouSure;
            SaveManager.NewSave += AddNewSave;
            showSavesAmount.NewShowAmount += AddAllSaves;
        }
#if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying) return;

            var saveButtons = content.GetComponentsInChildren<SaveButton>(true);
            foreach (SaveButton saveButton in saveButtons) 
                saveButton.gameObject.SetActive(false);
            preInstanced = saveButtons;
        }
#endif

        void OnDisable()
        {
            SaveButton.WantToDeleteSave -= ToggleAreYouSure;
            SaveManager.NewSave -= AddNewSave;
            showSavesAmount.NewShowAmount -= AddAllSaves;
        }

        void AddNewSave(FullSave obj, string path)
        {
            var move = Instantiate(saveBtn, content);
            move.transform.SetAsFirstSibling();
            move.Setup(obj, path);
            SaveManager.SavesDirty = false; // Handle directly
        }

        void ToggleAreYouSure(Action action) => areYouSure.Setup(action);

        void RefreshSaves()
        {
           // saves = Directory.GetFiles(SaveManager.SavePath).OrderByDescending(Directory.GetLastWriteTime);
            saves = new DirectoryInfo(SaveManager.SavePath).GetFiles()
                .OrderByDescending(f => f.LastWriteTime).Select(fileInfo => fileInfo.FullName).ToArray();
            AddAllSaves();
            SaveManager.SavesDirty = false;
        }

        void AddAllSaves()
        {
            foreach (SaveButton activeButton in activeButtons)
            {
                activeButton.Clear();
                activeButton.gameObject.SetActive(false);
                ButtonPool.Enqueue(activeButton);
            }
            activeButtons.Clear();
            for (int i = 0; i < GetShowAmount() && i < saves.Length; i++)
                TryAddSave(i);
        }

        private int GetShowAmount() => ShowSavesAmountSlider.ShowAmount + failed;

        private void TryAddSave(int i)
        {
            try
            {
                FullSave fullSave = JsonUtility.FromJson<FullSave>(File.ReadAllText(saves[i]));
                SaveButton button = GetButton();
                button.transform.SetSiblingIndex(i);
                button.Setup(fullSave,saves[i]);
                activeButtons.Add(button);
            }
            catch
            {
                Debug.LogWarning("Bad save file");
                failed++;
            }

        }
    }
}
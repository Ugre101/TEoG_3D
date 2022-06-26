using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SaveStuff
{
    public class StartSaveMenu : MonoBehaviour
    {
        [SerializeField] Transform content;
        [SerializeField] SaveButton saveBtn;
        [SerializeField] AreYouSure areYouSure;
        [SerializeField] Button clearAll;
        [SerializeField] ShowSavesAmountSlider showSavesAmount;
        [SerializeField] SaveButton[] btns;
        Queue<SaveButton> btnPool;
        int failed;
        IOrderedEnumerable<string> saves;

        void Start()
        {
            RefreshSaves();
            showSavesAmount.SetupSlider(saves.Count());
            clearAll.onClick.AddListener(ClearAllSaves);
            showSavesAmount.NewShowAmount += PrintSaves;
        }

        void OnEnable()
        {
            areYouSure.gameObject.SetActive(false);
            SaveButton.WantToDeleteSave += ToggleAreYouSure;
        }

        void OnDisable() => SaveButton.WantToDeleteSave -= ToggleAreYouSure;
        void OnDestroy() => showSavesAmount.NewShowAmount -= PrintSaves;
#if UNITY_EDITOR
        void OnValidate() => btns = content.GetComponentsInChildren<SaveButton>(true);
#endif
        SaveButton GetButton()
        {
            if (btnPool.Count <= 0)
                return Instantiate(saveBtn, content);
            var btn = btnPool.Dequeue();
            btn.gameObject.SetActive(true);
            return btn;
        }

        void SetupBtnPool()
        {
            btnPool = new Queue<SaveButton>();
            foreach (var item in btns)
            {
                btnPool.Enqueue(item);
                item.gameObject.SetActive(false);
            }
        }

        void ClearAllSaves() => areYouSure.Setup(ClearAllExceptLastTen);

        void ToggleAreYouSure(Action action) => areYouSure.Setup(action);

        void RefreshSaves()
        {
            saves = Directory.GetFiles(SaveManager.SavePath).OrderByDescending(Directory.GetLastWriteTime);
            PrintSaves();
        }

        void PrintSaves()
        {
            SetupBtnPool();
            var array = saves.ToArray();
            for (int i = 0; i < array.Length && i < GetShowAmount(); i++)
                SetupSave(array[i]);
        }

        int GetShowAmount() => ShowSavesAmountSlider.ShowAmount + failed;

        void SetupSave(string path)
        {
            try
            {
                FullSave fullSave = JsonUtility.FromJson<FullSave>(File.ReadAllText(path));
                GetButton().Setup(fullSave, path);
            }
            catch
            {
                Debug.LogWarning("Bad save file");
                failed++;
            }
        }

        void ClearAllExceptLastTen()
        {
            for (int i = saves.Count(); i-- > 10;)
                File.Delete(saves.ElementAt(i));
            RefreshSaves();
            showSavesAmount.NewMaxAmount(saves.Count());
        }
    }
}
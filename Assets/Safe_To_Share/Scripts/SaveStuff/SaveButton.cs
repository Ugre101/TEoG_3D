using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace SaveStuff {
    public sealed class SaveButton : MonoBehaviour {
        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TextMeshProUGUI text;

        [SerializeField] bool needsDoubleClick = true;

        // [SerializeField] Button loadBtn, deleteSaveBtn;
        bool firstClick = true;
        bool loading;
        string path;
        Save save;

        void OnDestroy() => LoadManager.LoadedSave -= SetLoadingFalse;
        public static event Action<Action> WantToDeleteSave;

        public void Setup(FullSave fullSave, string path) {
            this.path = path;
            var summary = fullSave.Summary;
            title.text = summary.PlayerAddedText;
            text.text = SummaryText(summary);
            save = fullSave.Save;
            // loadBtn.onClick.AddListener(LoadSave);
            // if (deleteSaveBtn != null)
            //    deleteSaveBtn.onClick.AddListener(DeleteSave);
            LoadManager.LoadedSave += SetLoadingFalse;
        }

        void SetLoadingFalse() => loading = false;

        static string SummaryText(SaveSummary summary) {
            StringBuilder sb = new();
            sb.AppendLine($"Player Name: {summary.PlayerName}");
            sb.AppendLine($"Player level: {summary.Level}");
            sb.AppendLine($"Map: {summary.SceneName}");
            sb.AppendLine(summary.Date);
            return sb.ToString();
        }

        void ClearSaveAndDeleteBtn() {
            File.Delete(path);
            Clear();
            gameObject.SetActive(false);
        }

        public void DeleteSave() => WantToDeleteSave?.Invoke(ClearSaveAndDeleteBtn);

        public void LoadSave() {
            if (loading)
                return;
            if (needsDoubleClick && firstClick) {
                firstClick = false;
                return;
            }

            loading = true;
            LoadManager.Instance.Load(save);
            //LoadMe?.Invoke(save);
        }

        public void Clear() => LoadManager.LoadedSave -= SetLoadingFalse;
    }
}
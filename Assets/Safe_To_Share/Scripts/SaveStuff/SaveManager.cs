using System;
using System.Globalization;
using System.IO;
using AvatarStuff.Holders;
using Character;
using Character.PlayerStuff;
using UnityEngine;

namespace SaveStuff
{
    public class SaveManager : MonoBehaviour
    {
        public static bool SavesDirty = true;
        public static Save? LastSave;

        [SerializeField] PlayerHolder playerHolder;
        Player Player => playerHolder.Player;

        public static string SavePath
        {
            get
            {
                string path = Path.Combine(Application.persistentDataPath, "Saves013");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        void Start() => NewSaveField.NewSave += NamedSave;

        void OnDestroy() => NewSaveField.NewSave -= NamedSave;

        public static event Action<FullSave, string> NewSave;

        void NamedSave(string obj) =>
            SaveGame($"{Player.Identity.FullName}{DateTime.Now.ToString(CultureInfo.CurrentCulture)}", obj);

        public void OnQuickSave() => QuickSave();
        public void OnQuickLoad() => LoadManager.Instance.QuickLoad();

        public void QuickSave() =>
            SaveGame($"QuickSave{Player.Identity.FullName}{DateTime.Now.ToString(CultureInfo.CurrentCulture)}");

        void SaveGame(string saveName, string addedText = "QuickSave")
        {
            string cleanSaveName = CleanSave(saveName);
            if (string.IsNullOrEmpty(cleanSaveName))
            {
                PromptErrorBadSaveName?.Invoke();
                return;
            }

            string fullSaveName = Path.Combine(SavePath, cleanSaveName);
            SaveSummary summary = new(Player.Identity.FullName, Player.LevelSystem.Level, addedText);
            Save newSave = new(new PlayerSave(new ControlledCharacterSave(Player), playerHolder.transform.position,
                Player.Inventory.Save()));
            LastSave = newSave;
            FullSave fullSave = new(summary, newSave);
            if (File.Exists(fullSaveName))
                PromptOverWrite(fullSaveName, fullSave);
            else
                FinishSave(fullSaveName, fullSave);
        }

        static void FinishSave(string fullSaveName, FullSave fullSave)
        {
            File.WriteAllText(fullSaveName, JsonUtility.ToJson(fullSave));
            SavesDirty = true;
            NewSave?.Invoke(fullSave, fullSaveName);
        }

        public static event Action PromptErrorBadSaveName;

        static string CleanSave(string saveName)
        {
            char[] cleaner = saveName.ToCharArray();
            cleaner = Array.FindAll(cleaner, char.IsLetterOrDigit);
            string cleanString = new(cleaner);
            return cleanString;
        }

        static void PromptOverWrite(string fullSaveName, FullSave newSave) => FinishSave(fullSaveName, newSave);
        // Spawn Yes & No message
        //  void Accept() => FinishSave(fullSaveName, newSave);
    }
}
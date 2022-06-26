using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvatarStuff
{
    public static class AvatarDetails
    {
        public static Dictionary<string, SavedValues> AvatarDetailsSavesDict = new();

        public static string Save()
        {
            List<SavedValues> returnList = AvatarDetailsSavesDict.Values.ToList();
            SaveWrapper final = new(returnList);
            return JsonUtility.ToJson(final);
        }

        public static void Load(string savedValuesList)
        {
            if (string.IsNullOrWhiteSpace(savedValuesList)) return;
            var parsed = JsonUtility.FromJson<SaveWrapper>(savedValuesList);
            if (parsed.savedValuesList.Count > 0)
                AvatarDetailsSavesDict = parsed.savedValuesList.ToDictionary(s => s.guid);
        }

        [Serializable]
        public struct SaveWrapper
        {
            public List<SavedValues> savedValuesList;
            public SaveWrapper(List<SavedValues> savedValuesList) => this.savedValuesList = savedValuesList;
        }

        [Serializable]
        public struct SavedValues
        {
            public string guid;
            public List<ColorSave> colorSaves;

            public SavedValues(string guid)
            {
                this.guid = guid;
                colorSaves = new List<ColorSave>();
            }

            public List<ColorSave> ColorSaves => colorSaves;

            public void MatToSave(Material mat)
            {
                string color = ColorUtility.ToHtmlStringRGBA(mat.color);
                ColorSaves.Add(new ColorSave(mat.name, color));
            }
        }

        [Serializable]
        public struct ColorSave
        {
            public string matName;
            public string colorName;

            public ColorSave(string matName, string colorName)
            {
                this.matName = matName;
                this.colorName = colorName;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public static class UgreTools
{
    public static void AwakeChildren(this Transform transform)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }

    public static void AwakeChildren(this Transform transform, params GameObject[] excepts)
    {
        foreach (Transform child in transform)
        {
            bool isException = excepts.Any(gameObject => ReferenceEquals(child.gameObject, gameObject));
            child.gameObject.SetActive(!isException);
        }
    }

    public static void SleepChildren(this Transform transform)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public static void SleepChildren(this Transform transform, GameObject except)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(ReferenceEquals(child.gameObject, except));
    }

    public static void KillChildren(this Transform transform)
    {
        foreach (Transform child in transform)
            Object.Destroy(child.gameObject);
    }

    public static T GetEnumFromPlayerPrefs<T>(string saveName, T altValue) where T : Enum =>
        PlayerPrefs.HasKey(saveName) &&
        Enum.IsDefined(typeof(T), Enum.ToObject(typeof(T), PlayerPrefs.GetInt(saveName)))
            ? (T)Enum.ToObject(typeof(T), PlayerPrefs.GetInt(saveName))
            : altValue;

    public static string DateToMonth(this DateTime month) =>
        month.ToString("dddd,MMMM dd yyy", CultureInfo.InvariantCulture);

    public static bool GreaterThanZero(this int value) => value > 0;

    public static IEnumerable<TEum> EnumToArray<TEum>(params TEum[] altExcludeOptions) where TEum : Enum
    {
        foreach (var e in Enum.GetValues(typeof(TEum)))
            if (e is TEum te && (altExcludeOptions == null || !altExcludeOptions.Contains(te)))
                yield return te;
    }

    public static TEum IntToEnum<TEum>(int value, TEum defaultTo, params TEum[] altExcludeOptions) where TEum : Enum
        => EnumToArray(altExcludeOptions).ElementAt(value) is { } match ? match : defaultTo;

    static List<TMP_Dropdown.OptionData> EnumToDataList<TEum>(params TEum[] altExcludeOptions) where TEum : Enum =>
        EnumToArray(altExcludeOptions).Select(EnumToData).ToList();

    static TMP_Dropdown.OptionData EnumToData<TEum>(TEum o) where TEum : Enum =>
        new(StringFormatting.AddSpaceAfterCapitalLetter(o.ToString()));

    public static void SetupTmpDropDown<TEum>(this TMP_Dropdown dropdown, TEum currentValue, UnityAction<int> onChange,
        params TEum[] altExcludeOptions) where TEum : Enum
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(EnumToDataList(altExcludeOptions));
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.value = IndexOfEnum(currentValue);
        dropdown.onValueChanged.AddListener(onChange);
    }

    public static bool ToggleSetActive(this GameObject gameObject)
    {
        bool newState = !gameObject.activeSelf;
        gameObject.SetActive(newState);
        return newState;
    }


    public static int IndexOfEnum<TEum>(this TEum value, params TEum[] altExcludeOptions) where TEum : Enum
        => EnumToArray(altExcludeOptions).ToList().IndexOf(value);

    public static string IntToFirstSecondEtc(this int value)
    {
        if (value <= 0)
            return value.ToString();

        return value switch
        {
            1 => "first",
            2 => "second",
            3 => "third",
            4 => "fourth",
            5 => "fifth",
            6 => "sixth",
            7 => "seventh",
            8 => "eighth",
            9 => "ninth",
            _ => AboveTeen(value),
        };

        static string AboveTeen(int value)
        {
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return $"{value}th";
            }

            return (value % 10) switch
            {
                1 => $"{value}st",
                2 => $"{value}nd",
                3 => $"{value}rd",
                _ => $"{value}th",
            };
        }
    }


    public static class StringFormatting
    {
        public static string AddSpaceAfterCapitalLetter(string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            StringBuilder sb = new(text.Length * 2);
            sb.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                bool shouldAddWhiteSpace = char.IsUpper(text[i]) &&
                                           ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                                            (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                                             i < text.Length - 1 && !char.IsUpper(text[i + 1])));
                if (shouldAddWhiteSpace)
                    sb.Append(' ');
                sb.Append(text[i]);
            }

            return sb.ToString();
        }

        public static string AddNewLineAfter(params string[] lines) =>
            lines.Aggregate(string.Empty, (current, line) => $"{current}{line}\n");

        public static string CleanFilePath(string path)
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            return string.Join("_", path.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }

    public static class Levenshtein
    {
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
                return m;

            if (m == 0)
                return n;

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            for (int j = 1; j <= m; j++)
            {
                // Compute cost.
                int cost = t[j - 1] == s[i - 1] ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }

            // Return cost.
            return d[n, m];
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Character.IdentityStuff
{
    [CreateAssetMenu(fileName = "Gender name list", menuName = "Character/Name list", order = 0)]
    public class GenderedNameList : ScriptableObject
    {
        [SerializeField] string[] neutralNames;
        [SerializeField] string[] femaleNames;
        [SerializeField] string[] malesNames;

        [Space(1f), SerializeField,] string[] lastNames;

        readonly Random rnd = new();


#if UNITY_EDITOR
        [Header("Editor Only")]
        [SerializeField] TextAsset nuetralNamesFile;
        [SerializeField] TextAsset femaleNamesFile;
        [SerializeField] TextAsset maleNamesFile;
        [SerializeField] TextAsset lastNamesFile;
        private void OnValidate()
        {
            neutralNames = nuetralNamesFile ? nuetralNamesFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) : null;
            femaleNames = femaleNamesFile ? femaleNamesFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) : null;
            malesNames = maleNamesFile ? maleNamesFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) : null;
            lastNames = lastNamesFile ? lastNamesFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) : null;
        }
#endif
        public string GetRandomFemaleName => GetRandomStringName(femaleNames);
        public string GetRandomMaleName => GetRandomStringName(malesNames);
        public string GetRandomNeutralName => GetRandomStringName(neutralNames);
        public string GetRandomLastName => GetRandomStringName(lastNames);

        string GetRandomStringName(IReadOnlyList<string> list) =>
            list.Count > 0 ? list[rnd.Next(0, list.Count)] : string.Empty;
    }
}
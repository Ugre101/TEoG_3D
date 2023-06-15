using System;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.BirthMenu {
    public sealed class BabyNameInput : MonoBehaviour {
        [SerializeField] TextMeshProUGUI preText;
        [SerializeField] TMP_InputField inputField;

        int nr;
        public event Action<string, int> NameChange;

        public void Setup(int babyNr, string s) {
            nr = babyNr;
            preText.text = $"{(babyNr + 1).IntToFirstSecondEtc()} Baby";
            inputField.SetTextWithoutNotify(s);
        }

        public void OnValueChange(string newString) => NameChange?.Invoke(newString, nr);
    }
}
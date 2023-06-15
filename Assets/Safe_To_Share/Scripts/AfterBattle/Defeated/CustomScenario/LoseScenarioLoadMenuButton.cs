using System;
using Character.DefeatScenarios.Custom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario {
    public sealed class LoseScenarioLoadMenuButton : MonoBehaviour {
        [SerializeField] Button btn;
        [SerializeField] TextMeshProUGUI btnTitle;
        CustomLoseScenario jsonData;
        public static event Action<CustomLoseScenario> LoadScenario;

        public void Setup(string title, CustomLoseScenario jsonData) {
            btnTitle.text = title;
            this.jsonData = jsonData;
            btn.onClick.AddListener(LoadMe);
        }

        void LoadMe() => LoadScenario?.Invoke(jsonData);
    }
}
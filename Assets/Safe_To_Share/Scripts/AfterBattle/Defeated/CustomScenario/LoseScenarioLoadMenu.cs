using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Character.DefeatScenarios.Custom;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario
{
    public class LoseScenarioLoadMenu : MonoBehaviour
    {
        [SerializeField] LoseScenarioLoadMenuButton prefab;
        [SerializeField] Transform container;

        void OnEnable()
        {
            Setup();
            LoseScenarioLoadMenuButton.LoadScenario += CloseMenu;
        }

        void OnDisable() => LoseScenarioLoadMenuButton.LoadScenario -= CloseMenu;

        void CloseMenu(CustomLoseScenario obj) => gameObject.SetActive(false);

        public void Setup()
        {
            if (!Directory.Exists(CustomLoseBuilderMenu.CustomScenarioFolder))
                return;
            var saves = Directory
                .GetFiles(CustomLoseBuilderMenu.CustomScenarioFolder, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".json", StringComparison.OrdinalIgnoreCase));
            SetupSaves(saves);
        }

        void SetupSaves(IEnumerable<string> saves)
        {
            container.KillChildren();
            foreach (string save in saves)
                SetupSaveBtn(save);
        }

        void SetupSaveBtn(string save)
        {
            var loadBtn = Instantiate(prefab, container);
            string fileName = save.Replace(CustomLoseBuilderMenu.CustomScenarioFolder, string.Empty)
                .Replace(".json", string.Empty).Replace("\\", string.Empty);
            CustomLoseScenario toLoad = JsonUtility.FromJson<CustomLoseScenario>(File.ReadAllText(save));
            loadBtn.Setup(fileName, toLoad);
        }
    }
}
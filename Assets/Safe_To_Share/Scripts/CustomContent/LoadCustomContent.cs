using System.Collections.Generic;
using System.IO;
using System.Linq;
using Character.CreateCharacterStuff;
using Character.DefeatScenarios.Custom;
using Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario;
using UnityEngine;

namespace Safe_To_Share.Scripts.CustomContent
{
    public class LoadCustomContent : MonoBehaviour
    {
        [SerializeField] List<EnemyPreset> enemyPresets = new();

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadCustomLoseScenarios();
        }

        void LoadCustomLoseScenarios()
        {
            string savePath = CustomLoseBuilderMenu.CustomScenarioFolder;
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            var files = Directory.GetFiles(savePath);
            foreach (var file in files)
                if (file.EndsWith(".json"))
                    LoadScenario(file);
        }

        void LoadScenario(string file)
        {
            var scenarioText = File.ReadAllText(file);
            var scenario = JsonUtility.FromJson<CustomLoseScenario>(scenarioText);
            if (scenario?.Enemies == null)
                return;
            foreach (EnemyPreset enemy in from enemy in enemyPresets
                from savedEnemy in
                    scenario.Enemies.Where(savedEnemy =>
                        savedEnemy == enemy.name && !enemy.customLoseScenarios.Contains(scenario))
                select enemy)
                FoundEnemy(scenario, enemy);
        }

        static void FoundEnemy(CustomLoseScenario scenario, EnemyPreset enemy)
        {
            if (!enemy.customLoseScenarios.Contains(scenario))
                enemy.customLoseScenarios.Add(scenario);
        }
    }
}
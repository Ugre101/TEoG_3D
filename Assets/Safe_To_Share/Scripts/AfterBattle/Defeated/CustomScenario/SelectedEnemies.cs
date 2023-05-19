using System.Collections.Generic;
using Character.CreateCharacterStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario
{
    public sealed class SelectedEnemies : MonoBehaviour
    {
        [SerializeField] CustomLoseBuilderMenu builderMenu;
        [SerializeField] SelectedEnemyButton prefab;
        [SerializeField] List<EnemyPreset> enemyPresets = new();
        [SerializeField] Transform content;

        void OnEnable()
        {
            if (builderMenu.loseScenario != null)
                PrintSelectableEnemies();
        }

        public void PrintSelectableEnemies()
        {
            content.KillChildren();
            foreach (var enemy in enemyPresets)
                AddEnemyButton(enemy);
        }

        void AddEnemyButton(EnemyPreset enemy)
        {
            var btn = Instantiate(prefab, content);
            btn.Setup(enemy.name, enemy.Desc);
            btn.btn.onClick.AddListener(Clicked);
            if (builderMenu.loseScenario.Enemies.Contains(enemy.name))
                btn.SetColor(Color.green);

            void Clicked()
            {
                if (builderMenu.loseScenario.Enemies.Contains(enemy.name))
                {
                    builderMenu.loseScenario.Enemies.Remove(enemy.name);
                    btn.SetColor(Color.white);
                    if (enemy.customLoseScenarios.Contains(builderMenu.loseScenario))
                        enemy.customLoseScenarios.Remove(builderMenu.loseScenario);
                }
                else
                {
                    builderMenu.loseScenario.Enemies.Add(enemy.name);
                    btn.SetColor(Color.green);
                    if (!enemy.customLoseScenarios.Contains(builderMenu.loseScenario))
                        enemy.customLoseScenarios.Add(builderMenu.loseScenario);
                }
            }
        }
    }
}
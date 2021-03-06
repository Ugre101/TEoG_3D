using Defeated;
using System.Collections.Generic;
using Character.DefeatScenarios.Custom;
using UnityEngine;

namespace Character.CreateCharacterStuff
{
    [CreateAssetMenu(menuName = "Create EnemyPreset", fileName = "EnemyPreset", order = 0)]
    public class EnemyPreset : CharacterPreset
    {
        [SerializeField] BattleReward battleReward;
        [SerializeField] CanTakeEnemyHome canTakeEnemyHome;
        [SerializeField] LoseScenario loseScenarios;
        [SerializeField] string desc;
        [SerializeField] bool giveRandomBodyMorphs;
        public string Desc => desc;
        public List<CustomLoseScenario> customLoseScenarios = new();
        public CreateEnemy NewEnemy() => new(NewCharacter(), battleReward, canTakeEnemyHome, loseScenarios.Guid, giveRandomBodyMorphs, customLoseScenarios);
    }
}
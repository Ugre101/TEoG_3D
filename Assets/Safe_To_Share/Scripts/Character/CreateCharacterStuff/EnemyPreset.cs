using System;
using System.Collections.Generic;
using Character.DefeatScenarios.Custom;
using Defeated;
using UnityEngine;

namespace Character.CreateCharacterStuff
{
    [CreateAssetMenu(menuName = "Character/Presets/Create EnemyPreset", fileName = "EnemyPreset")]
    public class EnemyPreset : CharacterPreset
    {
        [SerializeField] BattleReward battleReward;
        [SerializeField] CanTakeEnemyHome canTakeEnemyHome;
        [SerializeField] LoseScenario loseScenarios;
        [SerializeField] string desc;
        [SerializeField] bool giveRandomBodyMorphs;
        [field: SerializeField] public string EnemyGuid { get; private set; }
        public List<CustomLoseScenario> customLoseScenarios = new();
        public string Desc => desc;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(EnemyGuid))
                EnemyGuid = Guid.NewGuid().ToString();
        }
#endif

        public CreateEnemy NewEnemy() => new(NewCharacter(), battleReward, canTakeEnemyHome, loseScenarios.Guid,
            giveRandomBodyMorphs, customLoseScenarios, EnemyGuid);
    }
}
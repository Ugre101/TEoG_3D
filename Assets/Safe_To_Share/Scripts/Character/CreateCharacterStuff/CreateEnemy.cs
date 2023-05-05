using System.Collections.Generic;
using Character.DefeatScenarios.Custom;

namespace Character.CreateCharacterStuff
{
    public readonly struct CreateEnemy
    {
        public CreateEnemy(CreateCharacter character, BattleReward battleReward, CanTakeEnemyHome canTake,
                           string loseScenarios, bool giveRandomBodyMorphs, List<CustomLoseScenario> customScenarios,
                           string enemyGuid)
        {
            Character = character;
            BattleReward = battleReward;
            CanTake = canTake;
            LoseScenarios = loseScenarios;
            CustomLoseScenarios = customScenarios;
            GiveRandomBodyMorphs = giveRandomBodyMorphs;
            EnemyGuid = enemyGuid;
        }

        public CreateCharacter Character { get; }
        public BattleReward BattleReward { get; }
        public CanTakeEnemyHome CanTake { get; }
        public string LoseScenarios { get; }
        public List<CustomLoseScenario> CustomLoseScenarios { get; }
        public bool GiveRandomBodyMorphs { get; }
        public string EnemyGuid { get; }
    }
}
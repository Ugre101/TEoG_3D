using System.Collections.Generic;
using Character.DefeatScenarios.Custom;

namespace Character.CreateCharacterStuff
{
    public readonly struct CreateBoss
    {
        public CreateEnemy CreateEnemy { get; }
        public EnemyPreset UnderlingPreset { get; }
        public int UnderlingCount { get; }

        public CreateBoss(CreateEnemy createEnemy, EnemyPreset underlingPreset, int underlingCount)
        {
            CreateEnemy = createEnemy;
            UnderlingPreset = underlingPreset;
            UnderlingCount = underlingCount;
        }
    }
}
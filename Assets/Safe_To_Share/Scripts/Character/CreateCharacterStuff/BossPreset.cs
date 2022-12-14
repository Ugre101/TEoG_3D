using UnityEngine;

namespace Character.CreateCharacterStuff
{
    [CreateAssetMenu(menuName = "Character/Presets/Create Boss Preset", fileName = "BossPreset", order = 0)]
    public class BossPreset : EnemyPreset
    {
        [field: SerializeField] public EnemyPreset UnderlingPreset { get; private set; }
        [field: SerializeField] public int UnderlingCount { get; private set; }
        public CreateBoss NewBoss() => new(NewEnemy(), UnderlingPreset, UnderlingCount);
    }
}
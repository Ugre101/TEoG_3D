using Character.CreateCharacterStuff;
using Character.IslandData;

namespace Character.EnemyStuff
{
    public sealed class Boss : Enemy
    {
        public Boss(CreateBoss character) : base(character.CreateEnemy)
        {
        }

        public Boss(CreateBoss character, Islands islands) : base(character.CreateEnemy, islands)
        {
        }
    }
}
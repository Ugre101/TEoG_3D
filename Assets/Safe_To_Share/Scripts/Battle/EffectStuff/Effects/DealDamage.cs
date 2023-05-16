using System;
using Character;
using Character.StatsStuff.HealthStuff;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects
{
    [Serializable]
    public class DealDamage : Effect
    {
        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            Health health = target.Stats.Health;
            health.DecreaseCurrentValue(FinalIntValue(user, health.Value));
        }
    }
}
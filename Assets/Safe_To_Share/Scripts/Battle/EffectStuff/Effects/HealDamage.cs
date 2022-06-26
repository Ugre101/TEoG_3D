using System;
using Character;
using Character.StatsStuff.HealthStuff;

namespace Battle.EffectStuff.Effects
{
    [Serializable]
    public class HealDamage : Effect
    {
        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            Health health = target.Stats.Health;
            health.IncreaseCurrentValue(FinalIntValue(user, health.Value));
        }
    }
}
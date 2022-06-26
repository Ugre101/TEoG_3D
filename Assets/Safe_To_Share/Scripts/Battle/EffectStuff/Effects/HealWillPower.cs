using System;
using Character;
using Character.StatsStuff.HealthStuff;

namespace Battle.EffectStuff.Effects
{
    [Serializable]
    public class HealWillPower : Effect
    {
        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            Health willPower = target.Stats.WillPower;
            willPower.IncreaseCurrentValue(FinalIntValue(user, willPower.Value));
        }
    }
}
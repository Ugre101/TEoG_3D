using System;
using Character;
using Character.StatsStuff.HealthStuff;

namespace Battle.EffectStuff.Effects
{
    [Serializable]
    public class DealWillDamage : Effect
    {
        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            Health willPower = target.Stats.WillPower;
            willPower.DecreaseCurrentValue(FinalIntValue(user, willPower.Value));
        }
    }
}
using System;
using Character;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects {
    [Serializable]
    public class DealWillDamage : Effect {
        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            var willPower = target.Stats.WillPower;
            willPower.DecreaseCurrentValue(FinalIntValue(user, willPower.Value));
        }
    }
}
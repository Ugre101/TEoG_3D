using System;
using Character;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects {
    [Serializable]
    public class HealWillPower : Effect {
        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            var willPower = target.Stats.WillPower;
            willPower.IncreaseCurrentValue(FinalIntValue(user, willPower.Value));
        }
    }
}
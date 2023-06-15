using System;
using Character;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects {
    [Serializable]
    public class DealDamage : Effect {
        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            var health = target.Stats.Health;
            health.DecreaseCurrentValue(FinalIntValue(user, health.Value));
        }
    }
}
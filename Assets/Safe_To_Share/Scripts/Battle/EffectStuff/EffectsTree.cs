using System;
using System.Collections.Generic;
using Safe_To_Share.Scripts.Battle.EffectStuff.Effects;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff
{
    [Serializable]
    public class EffectsTree
    {
        [SerializeField] DealDamage dealDamage = new();
        [SerializeField] DealWillDamage dealWillDamage = new();
        [SerializeField] HealDamage healDamage = new();
        [SerializeField] HealWillPower healWillPower = new();
        [SerializeField] ShrinkBody shrinkBody = new();
        [SerializeField] GrowBody growBody = new();

        List<Effect> activeEffects;
        Effect[] effects;

        Effect[] Effects => effects ??= new Effect[]
        {
            dealDamage,
            dealWillDamage,
            healDamage,
            healWillPower,
            shrinkBody,
            growBody,
        };
        
        public List<Effect> ActiveEffects
        {
            get
            {
                if (activeEffects is not null) return activeEffects;
                activeEffects = new List<Effect>();
                foreach (var effect in Effects)
                    if (effect.Active)
                        activeEffects.Add(effect);

                return activeEffects;
            }
        }
    }
}
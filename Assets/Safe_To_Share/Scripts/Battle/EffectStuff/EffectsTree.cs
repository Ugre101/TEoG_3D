using System;
using System.Collections.Generic;
using Battle.EffectStuff.Effects;
using UnityEngine;

namespace Battle.EffectStuff
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
                if (activeEffects == null)
                {
                    activeEffects = new List<Effect>();
                    foreach (Effect effect in Effects)
                        if (effect.Active)
                            activeEffects.Add(effect);
                }

                return activeEffects;
            }
        }
    }
}
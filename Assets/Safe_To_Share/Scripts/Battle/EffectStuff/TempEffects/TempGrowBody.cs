using System;
using Battle.EffectStuff.Effects;
using Character;
using Character.BodyStuff;
using UnityEngine;

namespace Battle.EffectStuff.TempEffects
{
    [Serializable]
    public class TempGrowBody : TempEffect
    {
        [SerializeField] BodyStatType bodyType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            if (target.Body.BodyStats.TryGetValue(bodyType, out BodyStat body))
                body.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowBody)));
        }
    }
}
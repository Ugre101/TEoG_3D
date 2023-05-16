﻿using System;
using Character;
using Character.BodyStuff;
using Safe_To_Share.Scripts.Battle.EffectStuff.Effects;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.TempEffects
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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Animation
{
    [Serializable]
    public class SexAnimationFilterList
    {
        [SerializeField] SexActionAnimation defaultAnimation;
        [SerializeField,Tooltip("First is highest priority")] List<SexAnimationFilter> filters;

        public SexActionAnimation GetAnimation(AfterBattleActor caster, AfterBattleActor partner)
        {
            if (filters == null || filters.Count == 0)
                return defaultAnimation;
            foreach (var sexAnimationFilter in filters)
            {
                if (sexAnimationFilter.MeetsRequirements(caster,partner))
                    return sexAnimationFilter.Animation;
            }

            return defaultAnimation;
        }
    }
}
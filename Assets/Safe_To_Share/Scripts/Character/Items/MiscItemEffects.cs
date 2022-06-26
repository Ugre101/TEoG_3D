using System;
using System.Collections.Generic;
using Character;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class MiscItemEffects : ItemEffect
    {
        [SerializeField] List<TempIntMod> fatBurnMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (TempIntMod fatBurn in fatBurnMods)
                user.Body.FatBurnRate.Mods.AddTempStatMod(fatBurn);
        }
    }
}
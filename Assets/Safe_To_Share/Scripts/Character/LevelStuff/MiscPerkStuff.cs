using System;
using System.Collections.Generic;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.LevelStuff
{
    [Serializable]
    public class MiscPerkStuff
    {
        [SerializeField] List<IntMod> fatBurnMods = new();
        public void AssignMods(BaseCharacter character)
        {
            foreach (IntMod burnMod in fatBurnMods) 
                character.Body.FatBurnRate.Mods.AddStatMod(burnMod);
        }
    }
}
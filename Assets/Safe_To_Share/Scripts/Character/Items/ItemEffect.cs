using System;
using System.Collections.Generic;
using Character;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Items {
    [Serializable]
    public abstract class ItemEffect {
        [SerializeField] bool active;

        public bool Active => active;

        public abstract void OnUse(BaseCharacter user, string itemGuid);


        [Serializable]
        protected struct AssignTempMod {
            public List<TempIntMod> mod;

            readonly TempIntMod AddTitle(TempIntMod tempIntMod, string itemGuid) =>
                new(tempIntMod.HoursLeft,
                    tempIntMod.ModValue, itemGuid, tempIntMod.ModType);

            public readonly void AddMods(ModsContainer container, string itemGuid) {
                foreach (var tempIntMod in mod)
                    container.AddTempStatMod(AddTitle(tempIntMod, itemGuid));
            }
        }
    }
}
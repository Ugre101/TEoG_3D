using System;
using Character;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class PregnancyItemEffect : ItemEffect {
        [SerializeField] AssignTempMod fertilityMods;
        [SerializeField] AssignTempMod virilityMods;
        [SerializeField] AssignTempMod growthSpeedMods;
        [SerializeField] int permFertility;
        [SerializeField] int permVirility;

        public override void OnUse(BaseCharacter user, string itemGuid) {
            fertilityMods.AddMods(user.PregnancySystem.Fertility.Mods, itemGuid);
            virilityMods.AddMods(user.PregnancySystem.Virility.Mods, itemGuid);
            growthSpeedMods.AddMods(user.PregnancySystem.PregnancySpeed.Mods, itemGuid);
            if (permFertility != 0)
                user.PregnancySystem.Fertility.BaseValue += permFertility;
            if (permVirility != 0)
                user.PregnancySystem.Virility.BaseValue += permVirility;
        }
    }
}
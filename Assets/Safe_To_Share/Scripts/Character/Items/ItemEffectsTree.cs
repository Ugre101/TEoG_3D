using System;
using System.Collections.Generic;
using Safe_To_Share.Scripts.Character.Items.Item_Effects;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class ItemEffectsTree
    {
#if UNITY_EDITOR
        public static string[] PropertyNames =
        {
            "healingItemEffects",
            "pregnancyItemEffect",
            "bodyStatItemEffect",
            "voreItemEffect",
            "sexStatsItemEffects",
            "learnAbilityItemEffect",
            "raceEssenceItemEffects",
            "charStatItemEffects",
            "healthStatItemEffects",
            "sexualFluidItemEffects",
            "miscItemEffects",
        };
#endif
        [SerializeField] HealingItemEffects healingItemEffects = new();
        [SerializeField] PregnancyItemEffect pregnancyItemEffect = new();
        [SerializeField] BodyStatItemEffect bodyStatItemEffect = new();
        [SerializeField] VoreItemEffect voreItemEffect = new();
        [SerializeField] SexStatsItemEffects sexStatsItemEffects = new();
        [SerializeField] LearnAbilityItemEffect learnAbilityItemEffect = new();
        [SerializeField] RaceEssenceItemEffects raceEssenceItemEffects = new();
        [SerializeField] CharStatItemEffects charStatItemEffects = new();
        [SerializeField] HealthStatItemEffects healthStatItemEffects = new();
        [SerializeField] SexualFluidItemEffects sexualFluidItemEffects = new();
        [SerializeField] MiscItemEffects miscItemEffects = new();
        [SerializeField] SexualOrganItemEffect sexualOrganItemEffect = new();
        List<ItemEffect> activeEffects;
        ItemEffect[] allEffects;

        ItemEffect[] AllEffects => allEffects ??= new ItemEffect[]
        {
            healingItemEffects,
            pregnancyItemEffect,
            bodyStatItemEffect,
            voreItemEffect,
            learnAbilityItemEffect,
            sexStatsItemEffects,
            raceEssenceItemEffects,
            charStatItemEffects,
            healthStatItemEffects,
            sexualFluidItemEffects,
            miscItemEffects,
            sexualOrganItemEffect,
        };

        public List<ItemEffect> ActiveEffects
        {
            get
            {
                if (activeEffects != null) return activeEffects;
                activeEffects = new List<ItemEffect>();
                foreach (var effect in AllEffects)
                    if (effect.Active)
                        activeEffects.Add(effect);
                return activeEffects;
            }
        }
    }
}
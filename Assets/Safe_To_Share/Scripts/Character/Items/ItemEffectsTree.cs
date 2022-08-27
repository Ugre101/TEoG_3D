using System;
using System.Collections.Generic;
using Character;
using Character.BodyStuff;
using Character.Organs;
using Character.Race.Races;
using Character.StatsStuff;
using Character.StatsStuff.HealthStuff;
using Character.StatsStuff.Mods;
using CustomClasses;
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
        };

        public List<ItemEffect> ActiveEffects
        {
            get
            {
                if (activeEffects == null)
                {
                    activeEffects = new List<ItemEffect>();
                    foreach (ItemEffect effect in AllEffects)
                        if (effect.Active)
                            activeEffects.Add(effect);
                }

                return activeEffects;
            }
        }
    }

    [Serializable]
    public abstract class ItemEffect
    {
        [SerializeField] bool active;

        public bool Active => active;

        public abstract void OnUse(BaseCharacter user, string itemGuid);


        [Serializable]
        protected struct AssignTempMod
        {
            public List<TempIntMod> mod;

            readonly TempIntMod AddTitle(TempIntMod tempIntMod, string itemGuid) => new(tempIntMod.HoursLeft,
                tempIntMod.ModValue, itemGuid, tempIntMod.ModType);

            public readonly void AddMods(ModsContainer container, string itemGuid)
            {
                foreach (TempIntMod tempIntMod in mod)
                    container.AddTempStatMod(AddTitle(tempIntMod, itemGuid));
            }
        }
    }

    [Serializable]
    public class PregnancyItemEffect : ItemEffect
    {
        [SerializeField] AssignTempMod fertilityMods;
        [SerializeField] AssignTempMod virilityMods;
        [SerializeField] AssignTempMod growthSpeedMods;
        [SerializeField] int permFertility;
        [SerializeField] int permVirility;

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            fertilityMods.AddMods(user.PregnancySystem.Fertility.Mods, itemGuid);
            virilityMods.AddMods(user.PregnancySystem.Virility.Mods, itemGuid);
            growthSpeedMods.AddMods(user.PregnancySystem.PregnancySpeed.Mods, itemGuid);
            if (permFertility != 0)
                user.PregnancySystem.Fertility.BaseValue += permFertility;
            if (permVirility != 0)
                user.PregnancySystem.Virility.BaseValue += permVirility;
        }
    }

    [Serializable]
    public class BodyStatItemEffect : ItemEffect
    {
        [SerializeField] List<AssignBodyTempMod> bodyTempMods = new();
        [SerializeField] List<BodyChange> bodyChanges = new();

        [Header("Thickset"), SerializeField, Range(-1f, 1f),]
        float thickSetChange;

        [SerializeField] List<TempIntMod> tempThickMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (AssignBodyTempMod assignBodyTempMod in bodyTempMods)
                if (user.Body.BodyStats.TryGetValue(assignBodyTempMod.bodyStatType, out BodyStat bodyStat))
                    assignBodyTempMod.assignTempMod.AddMods(bodyStat.Mods, itemGuid);
            foreach (BodyChange bodyChange in bodyChanges)
                if (user.Body.BodyStats.TryGetValue(bodyChange.bodyStatType, out BodyStat bodyStat))
                    bodyStat.BaseValue += bodyChange.permChange;
            if (thickSetChange != 0)
                user.Body.Thickset.BaseValue += thickSetChange;
            foreach (TempIntMod tempThickMod in tempThickMods)
                user.Body.Thickset.Mods.AddTempStatMod(tempThickMod);
        }

        [Serializable]
        struct AssignBodyTempMod
        {
            public BodyStatType bodyStatType;
            public AssignTempMod assignTempMod;
        }

        [Serializable]
        struct BodyChange
        {
            public BodyStatType bodyStatType;
            public float permChange;
        }
    }

    [Serializable]
    public class VoreItemEffect : ItemEffect
    {
        [SerializeField] AssignTempMod digestionMods;
        [SerializeField] AssignTempMod capacityMods;
        [SerializeField] AssignTempMod pleasureMods;
        [SerializeField] AssignTempMod pleasureDrainModes;

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            digestionMods.AddMods(user.Vore.digestionStrength.Mods, itemGuid);
            capacityMods.AddMods(user.Vore.capacityBoost, itemGuid);
            pleasureMods.AddMods(user.Vore.pleasureDigestion.Mods, itemGuid);
            pleasureDrainModes.AddMods(user.Vore.orgasmDrain.Mods, itemGuid);
        }
    }

    [Serializable]
    public class HealingItemEffects : ItemEffect
    {
        [SerializeField] int hpGain, wpGain;

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            if (hpGain > 0)
                user.Stats.Health.IncreaseCurrentValue(hpGain);
            if (wpGain > 0)
                user.Stats.WillPower.IncreaseCurrentValue(wpGain);
        }
    }

    [Serializable]
    public class LearnAbilityItemEffect : ItemEffect
    {
        [SerializeField] DropSerializableObject<SerializableScriptableObject> dropSerializableObject;

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            if (user is ControlledCharacter controlledCharacter)
                controlledCharacter.AndSpellBook.LearnAbility(dropSerializableObject.guid);
        }
    }

    [Serializable]
    public class SexStatsItemEffects : ItemEffect
    {
        [SerializeField] List<AssignTempMod> orgMods = new();
        [SerializeField] List<AssignTempMod> arousalMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (AssignTempMod assignTempMod in orgMods)
                assignTempMod.AddMods(user.SexStats.MaxCasterOrgasms.Mods, itemGuid);
            foreach (AssignTempMod assignTempMod in arousalMods)
                assignTempMod.AddMods(user.SexStats.BaseMaxArousal.Mods, itemGuid);
        }
    }

    [Serializable]
    public class RaceEssenceItemEffects : ItemEffect
    {
        [SerializeField] BasicRace race;
        [SerializeField, Range(1, 250),] int toGain;

        public override void OnUse(BaseCharacter user, string itemGuid) => user.RaceSystem.AddRace(race, toGain);
    }

    [Serializable]
    public class CharStatItemEffects : ItemEffect
    {
        [SerializeField] List<AssignCharStatTempMod> assignCharStatTempMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (AssignCharStatTempMod statTempMod in assignCharStatTempMods)
                if (user.Stats.GetCharStats.TryGetValue(statTempMod.statType, out var stat))
                    statTempMod.tempMods.AddMods(stat.Mods, itemGuid);
        }

        [Serializable]
        struct AssignCharStatTempMod
        {
            public CharStatType statType;
            public AssignTempMod tempMods;
        }
    }

    [Serializable]
    public class HealthStatItemEffects : ItemEffect
    {
        [SerializeField] List<AssignHealthStatTempMod> assignCharStatTempMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (AssignHealthStatTempMod statTempMod in assignCharStatTempMods)
                switch (statTempMod.healthTypes)
                {
                    case HealthTypes.Health:
                        statTempMod.tempMods.AddMods(user.Stats.Health.Mods, itemGuid);
                        break;
                    case HealthTypes.WillPower:
                        statTempMod.tempMods.AddMods(user.Stats.WillPower.Mods, itemGuid);
                        break;
                    case HealthTypes.HealthRecovery:
                        statTempMod.tempMods.AddMods(user.Stats.Health.IntRecovery.Mods, itemGuid);
                        break;
                    case HealthTypes.WillPowerRecovery:
                        statTempMod.tempMods.AddMods(user.Stats.WillPower.IntRecovery.Mods, itemGuid);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        [Serializable]
        struct AssignHealthStatTempMod
        {
            public HealthTypes healthTypes;
            public AssignTempMod tempMods;
        }
    }

    [Serializable]
    public class SexualFluidItemEffects : ItemEffect
    {
        [SerializeField] List<FluidCapOfType> fluidCapStretch = new();
        [SerializeField] List<FluidRecOfType> fluidRecMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (FluidCapOfType fluidCapOfType in fluidCapStretch)
                if (user.SexualOrgans.Containers.TryGetValue(fluidCapOfType.organType, out var container))
                    fluidCapOfType.mods.AddMods(container.Fluid.Mods, itemGuid);

            foreach (FluidRecOfType fluidRecOfType in fluidRecMods)
                if (user.SexualOrgans.Containers.TryGetValue(fluidRecOfType.organType, out var container))
                    fluidRecOfType.mods.AddMods(container.Fluid.Recovery.Mods, itemGuid);
        }

        [Serializable]
        struct FluidCapOfType
        {
            public SexualOrganType organType;
            public AssignTempMod mods;
        }

        [Serializable]
        struct FluidRecOfType
        {
            public SexualOrganType organType;
            public AssignTempMod mods;
        }
    }
}
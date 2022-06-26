using System.Collections.Generic;
using System.Linq;
using Character.BodyStuff;
using Character.Organs;
using Character.Organs.Fluids;
using Character.PlayerStuff;
using Character.PregnancyStuff;
using Character.StatsStuff;
using Character.StatsStuff.Mods;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.LevelStuff
{
    [CreateAssetMenu(fileName = "Create PerkInfo", menuName = "Character/PerkInfo", order = 0)]
    public class BasicPerk : SObjSavableTitleDescIcon
    {
        [SerializeField, Range(1, 10),] int cost = 1;
        [SerializeField] PerkType perkType = PerkType.Basic;
        [SerializeField, HideInInspector,] List<string> needPerkGuids = new();
        [SerializeField, HideInInspector,] List<string> exclusiveWithPerkGuids = new();

        [SerializeField] List<RequireCharStat> requireCharStats = new();
        [SerializeField] List<AssignCharStatMod> statMods = new();
        [SerializeField] List<AssignBodyMod> bodyMods = new();
        [SerializeField] List<IntMod> recoveryMods = new();
        [SerializeField] List<AssignModsToOrganContainer> assignModsToOrganContainer = new();
        [SerializeField] AssignPregnancyMods assignPregnancyMods = new();
        [SerializeField] AssignFluidMods assignFluidMods = new();
        [SerializeField] MiscPerkStuff miscPerkStuff = new();
        public virtual PerkType PerkType => perkType;
        public int Cost => cost;
        public List<string> NeedPerkGuids => needPerkGuids;
        public List<string> ExclusiveWithPerkGuids => exclusiveWithPerkGuids;
        public List<RequireCharStat> RequireCharStats => requireCharStats;

        // Stat req
        public bool MeetsRequirements(BaseCharacter character)
        {
            if (NeedPerkGuids.Any(perkGuid =>
                    character.LevelSystem.OwnedPerks.FirstOrDefault(p => p.Guid == perkGuid) == null) &&
                NeedPerkGuids.Any(perkGuid =>
                    character.Essence.EssencePerks.FirstOrDefault(p => p.Guid == perkGuid) == null) &&
                NeedPerkGuids.Any(perkGuid =>
                    character.Vore.Level.OwnedPerks.FirstOrDefault(p => p.Guid == perkGuid) == null))
                return false;

            if (ExclusiveWithPerkGuids.Any(perkGuid =>
                    character.LevelSystem.OwnedPerks.FirstOrDefault(p => p.Guid == perkGuid) != null) ||
                ExclusiveWithPerkGuids.Any(perkGuid =>
                    character.Essence.EssencePerks.FirstOrDefault(p => p.Guid == perkGuid) != null) ||
                ExclusiveWithPerkGuids.Any(perkGuid =>
                    character.Vore.Level.OwnedPerks.FirstOrDefault(p => p.Guid == perkGuid) != null))
                return false;

            return RequireCharStats.All(charStat =>
                character.Stats.GetCharStats[charStat.StatType].BaseValue >= charStat.Amount);
        }

        /// <summary>
        ///     Called when gained and after load. Stuff like stat mods are added here
        /// </summary>
        public virtual void PerkGainedEffect(BaseCharacter character)
        {
            foreach (AssignCharStatMod charStatMod in statMods)
                if (character.Stats.GetCharStats.TryGetValue(charStatMod.Stat, out CharStat stat))
                    stat.Mods.AddStatMod(charStatMod.Mod);
            foreach (AssignBodyMod bodyMod in bodyMods)
                if (character.Body.BodyStats.TryGetValue(bodyMod.Type, out BodyStat body))
                    body.Mods.AddStatMod(bodyMod.Mod);
            foreach (IntMod recoveryMod in recoveryMods)
            {
                character.Stats.Health.Mods.AddStatMod(recoveryMod);
                character.Stats.WillPower.Mods.AddStatMod(recoveryMod);
            }

            foreach (AssignModsToOrganContainer modsToOrganContainer in assignModsToOrganContainer)
                modsToOrganContainer.Assign(character);
            assignPregnancyMods.AssignMods(character);
            assignFluidMods.AssignMods(character);
            miscPerkStuff.AssignMods(character);
        }
#if UNITY_EDITOR
        [SerializeField] List<BasicPerk> needPerk = new();
        [SerializeField] List<BasicPerk> exclusiveWithPerk = new();
        public override void OnValidate()
        {
            base.OnValidate();
            needPerkGuids = new List<string>();
            foreach (BasicPerk basicPerk in needPerk)
                NeedPerkGuids.Add(basicPerk.Guid);
            exclusiveWithPerkGuids = new List<string>();
            foreach (BasicPerk basicPerk in exclusiveWithPerk)
                ExclusiveWithPerkGuids.Add(basicPerk.Guid);
        }
#endif
    }
}
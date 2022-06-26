using System.Collections.Generic;
using Character.LevelStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.EssenceStuff
{
    [CreateAssetMenu(fileName = "Create EssencePerk", menuName = "Character/EssencePerk")]
    public class EssencePerk : BasicPerk
    {
        [SerializeField, Min(0),] int loseFlatBonus;
        [SerializeField, Range(0, 100),] int lostPercentBonus;
        [SerializeField] List<IntMod> stableEssenceMods = new();
        [SerializeField] List<IntMod> drainEssenceMods = new();
        [SerializeField] List<IntMod> giveEssenceMods = new();
        public override PerkType PerkType => PerkType.Essence;

        public int LoseFlatBonus => loseFlatBonus;

        public int LostPercentBonus => lostPercentBonus;

        public override void PerkGainedEffect(BaseCharacter character)
        {
            EssenceSystem essence = character.Essence;
            foreach (IntMod essenceMod in stableEssenceMods)
                essence.StableEssence.Mods.AddStatMod(essenceMod);
            foreach (IntMod essenceMod in drainEssenceMods)
                essence.DrainAmount.Mods.AddStatMod(essenceMod);
            foreach (IntMod essenceMod in giveEssenceMods)
                essence.GiveAmount.Mods.AddStatMod(essenceMod);
        }

        public virtual void OnCasterOrgasmPerkEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
        }

        public virtual void OnPartnerOrgasmPerkEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
        }

        public virtual void PerkDrainEssenceEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
        }

        public virtual void PerkGetDrainedEssenceEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
        }

        public virtual void PerkGiveEssenceEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
        }
    }
}
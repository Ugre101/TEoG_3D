using Character.LevelStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.SexStatsStuff
{
    [CreateAssetMenu(fileName = "New sexperk", menuName = "Character/SexPerk", order = 0)]
    public class SexPerk : BasicPerk
    {
        [SerializeField] IntMod[] maxOrgasmsMod;
        public override void PerkGainedEffect(BaseCharacter character)
        {
            base.PerkGainedEffect(character);
            foreach (IntMod intMod in maxOrgasmsMod) 
                character.SexStats.MaxCasterOrgasms.Mods.AddStatMod(intMod);
        }
    }
}
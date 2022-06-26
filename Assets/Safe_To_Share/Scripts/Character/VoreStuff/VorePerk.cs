using System.Collections.Generic;
using System.Linq;
using Character.LevelStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.VoreStuff
{
    [CreateAssetMenu(fileName = "Create VorePerk", menuName = "Character/Vore/VorePerk", order = 0)]
    public class VorePerk : BasicPerk
    {
        [SerializeField] List<IntMod> capacityMods;
        [SerializeField] List<IntMod> digestionMods;
        [SerializeField] List<IntMod> pleasureDigestionMods;
        [SerializeField] List<IntMod> drainMods;
        public override PerkType PerkType => PerkType.Vore;

        public override void PerkGainedEffect(BaseCharacter character)
        {
            base.PerkGainedEffect(character);
            foreach (IntMod capacityMod in capacityMods) 
                character.Vore.capacityBoost.AddStatMod(capacityMod);
            foreach (IntMod digestionMod in digestionMods)
                character.Vore.digestionStrength.Mods.AddStatMod(digestionMod);
            foreach (IntMod pleasureMods in pleasureDigestionMods)
                character.Vore.pleasureDigestion.Mods.AddStatMod(pleasureMods);
            foreach (IntMod drainMod in drainMods)
                character.Vore.orgasmDrain.Mods.AddStatMod(drainMod);
        }

        public virtual void OnTick(BaseCharacter character)
        {
        }

        public virtual void OnDigestion(BaseCharacter pred, BaseCharacter prey)
        {
        }
    }
    public static class VorePerksExtensions
    {
        public static void GainPerk(this VorePerk perk,BaseCharacter baseCharacter)
        {
            if (baseCharacter.Vore.Level.OwnedPerks.Contains(perk))
                return;
            baseCharacter.Vore.Level.OwnedPerks.Add(perk);
            perk.PerkGainedEffect(baseCharacter);
        }
    }
}
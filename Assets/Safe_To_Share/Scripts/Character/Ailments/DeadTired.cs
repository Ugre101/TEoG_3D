using System.Linq;
using Character.StatsStuff.Mods;

namespace Character.Ailments {
    public sealed class DeadTired : Ailment {
        const string Cause = "DeadTired";

        public DeadTired() : base(-3, Cause, ModType.Flat) { }

        public static bool Has(BaseCharacter character) =>
            character.Stats.GetCharStats.Values.Any(stat => stat.Mods.StatMods.Exists(m => m.From == Cause)) ||
            character.SexStats.MaxCasterOrgasms.Mods.StatMods.Exists(m => m.From == Cause);

        public override bool Gain(BaseCharacter character) {
            var change = false;
            foreach (var stat in character.Stats.GetCharStats.Values.Where(stat => !stat.Mods.HaveModFrom(Cause))) {
                stat.Mods.AddStatMod(this);
                change = true;
            }

            if (!character.SexStats.MaxCasterOrgasms.Mods.HaveModFrom(Cause)) {
                character.SexStats.MaxCasterOrgasms.Mods.AddStatMod(new IntMod(-1, Cause, ModType.Flat));
                change = true;
            }

            return change;
        }

        public override bool Cure(BaseCharacter character) {
            var change = false;
            foreach (var unused in character.Stats.GetCharStats.Values.Where(stat =>
                         stat.Mods.RemoveStatModsFromSource(Cause)))
                change = true;
            if (character.SexStats.MaxCasterOrgasms.Mods.RemoveStatModsFromSource(Cause))
                change = true;

            return change;
        }
    }
}
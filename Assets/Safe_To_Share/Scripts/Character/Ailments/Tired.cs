using System.Linq;
using Character.StatsStuff.Mods;

namespace Character.Ailments {
    public sealed class Tired : Ailment {
        const string Cause = "Tired";

        public Tired() : base(-1, Cause, ModType.Flat) { }

        public static bool Has(BaseCharacter character) =>
            character.Stats.GetCharStats.Values.Any(stat => stat.Mods.HaveModFrom(Cause));

        public override bool Gain(BaseCharacter character) {
            var change = false;
            foreach (var stat in character.Stats.GetCharStats.Values.Where(stat => !stat.Mods.HaveModFrom(Cause))) {
                stat.Mods.AddStatMod(this);
                change = true;
            }

            return change;
        }

        public override bool Cure(BaseCharacter character) {
            var change = false;
            foreach (var unused in character.Stats.GetCharStats.Values.Where(stat =>
                         stat.Mods.RemoveStatModsFromSource(Cause)))
                change = true;
            return change;
        }
    }
}
using Character.StatsStuff.Mods;

namespace Character.Ailments {
    public sealed class Hungry : Ailment {
        const string Cause = "Hungry";

        public Hungry() : base(-1, Cause, ModType.Flat) { }

        public static bool Has(BaseCharacter character) =>
            character.Stats.Health.IntRecovery.Mods.StatMods.Exists(m => m.From == Cause) ||
            character.Stats.WillPower.IntRecovery.Mods.StatMods.Exists(m => m.From == Cause);

        public override bool Gain(BaseCharacter character) {
            var change = false;
            if (!character.Stats.Health.IntRecovery.Mods.HaveModFrom(Cause)) {
                character.Stats.Health.IntRecovery.Mods.AddStatMod(this);
                change = true;
            }

            if (!character.Stats.WillPower.IntRecovery.Mods.HaveModFrom(Cause)) {
                character.Stats.WillPower.IntRecovery.Mods.AddStatMod(this);
                change = true;
            }

            return change;
        }

        public override bool Cure(BaseCharacter character) =>
            character.Stats.Health.IntRecovery.Mods.RemoveStatModsFromSource(Cause) |
            character.Stats.WillPower.IntRecovery.Mods.RemoveStatModsFromSource(Cause);
    }
}
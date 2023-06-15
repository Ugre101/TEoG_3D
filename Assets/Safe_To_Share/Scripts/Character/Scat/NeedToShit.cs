using Character;
using Character.Ailments;
using Character.StatsStuff.Mods;

namespace Safe_To_Share.Scripts.Character.Scat {
    public sealed class NeedToShit : Ailment {
        const string Cause = "NeedToShit";

        public NeedToShit() : base(-1, Cause, ModType.Flat) { }

        public static bool Has(BaseCharacter character) => character.Stats.Agility.Mods.HaveModFrom(Cause);

        public override bool Gain(BaseCharacter character) {
            var change = false;
            if (character.Stats.Agility.Mods.HaveModFrom(Cause) is false) {
                character.Stats.Agility.Mods.AddStatMod(this);
                change = true;
            }

            return change;
        }

        public override bool Cure(BaseCharacter character) {
            var change = false;
            if (character.Stats.Agility.Mods.HaveModFrom(Cause)) {
                character.Stats.Agility.Mods.RemoveStatModsFromSource(Cause);
                change = true;
            }

            return change;
        }
    }
}
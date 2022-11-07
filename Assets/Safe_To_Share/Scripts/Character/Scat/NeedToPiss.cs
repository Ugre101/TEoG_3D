using Character;
using Character.Ailments;
using Character.StatsStuff.Mods;

namespace Safe_To_Share.Scripts.Character.Scat
{
    public class NeedToPiss : Ailment
    {
        const string Cause = "NeedToPiss";

        public NeedToPiss() : base(-1, Cause, ModType.Flat)
        {
        }

        public override bool Gain(BaseCharacter character)
        {
            var change = false;
            if (character.Stats.Charisma.Mods.HaveModFrom(Cause) is false)
            {
                character.Stats.Charisma.Mods.AddStatMod(this);
                change = true;
            }

            return change;
        }

        public override bool Cure(BaseCharacter character)
        {
            var change = false;
            if (character.Stats.Charisma.Mods.HaveModFrom(Cause))
            {
                character.Stats.Charisma.Mods.RemoveStatModsFromSource(Cause);
                change = true;
            }

            return change;
        }
    }
}
using Character.StatsStuff.Mods;

namespace Character.Ailments
{
    public class FluidStretch : Ailment
    {
        const string Cause = "FluidStretch";
        public FluidStretch() : base(20, Cause, ModType.Percent)
        {
        }

        public override bool Gain(BaseCharacter character)
        {
            if (!character.SexStats.GainArousalFactor.Mods.HaveModFrom(Cause))
            {
                character.SexStats.GainArousalFactor.Mods.AddStatMod(this);
                return true;
            }

            return false;
        }

        public override bool Cure(BaseCharacter character)
        {
            if (character.SexStats.GainArousalFactor.Mods.HaveModFrom(Cause))
            {
                character.SexStats.GainArousalFactor.Mods.RemoveStatModsFromSource(Cause);
                return true;
            }
            // Cure
            return false;
        }
    }
}
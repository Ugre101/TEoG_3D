using System.Linq;
using Character.StatsStuff;
using Character.StatsStuff.Mods;

namespace Character.Ailments
{
    public class Tired : Ailment
    {
        const string Cause = "Tired";

        public static bool Has(BaseCharacter character) => character.Stats.GetCharStats.Values.Any(stat => stat.Mods.HaveModFrom(Cause));

        public Tired() : base(-1, Cause, ModType.Flat)
        {
        }

        public override bool Gain(BaseCharacter character)
        {
            bool change = false;
            foreach (CharStat stat in character.Stats.GetCharStats.Values.Where(stat => !stat.Mods.HaveModFrom(Cause)))
            {
                stat.Mods.AddStatMod(this);
                change = true;
            }

            return change;
        }

        public override bool Cure(BaseCharacter character)
        {
            bool change = false;
            foreach (CharStat unused in character.Stats.GetCharStats.Values.Where(stat => stat.Mods.RemoveStatModsFromSource(Cause)))
                change = true;
            return change;
        }
    }
}
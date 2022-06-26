using System.Linq;
using Character.StatsStuff;
using Character.StatsStuff.Mods;

namespace Character.Ailments
{
    public class DeadTired : Ailment
    {
        const string Cause = "DeadTired";
        public static bool Has(BaseCharacter character) => 
            character.Stats.GetCharStats.Values.Any(stat => stat.Mods.StatMods.Exists(m => m.From == Cause)) || 
            character.SexStats.MaxCasterOrgasms.Mods.StatMods.Exists(m => m.From == Cause);

        public DeadTired() : base(-3, Cause, ModType.Flat)
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

            if (!character.SexStats.MaxCasterOrgasms.Mods.HaveModFrom(Cause))
            {
                character.SexStats.MaxCasterOrgasms.Mods.AddStatMod(new IntMod(-1,Cause,ModType.Flat));
                change = true;
            }

            return change;        }

        public override bool Cure(BaseCharacter character)
        {
            bool change = false;
            foreach (CharStat unused in character.Stats.GetCharStats.Values.Where(stat => stat.Mods.RemoveStatModsFromSource(Cause)))
                change = true;
            if (character.SexStats.MaxCasterOrgasms.Mods.RemoveStatModsFromSource(Cause))
                change = true;

            return change;        
        }
    }
}
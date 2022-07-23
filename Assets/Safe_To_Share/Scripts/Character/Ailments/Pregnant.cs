using System;
using Character.StatsStuff.Mods;

namespace Character.Ailments
{
    public class Pregnant : Ailment
    {
        public Pregnant(int modValue, string from, ModType modType) : base(modValue, from, modType)
        {
        }

        public override bool Gain(BaseCharacter character) => throw new NotImplementedException();

        public override bool Cure(BaseCharacter character) => throw new NotImplementedException();
    }
}
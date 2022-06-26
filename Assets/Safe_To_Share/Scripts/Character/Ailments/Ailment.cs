using System;
using Character.StatsStuff.Mods;

namespace Character.Ailments
{
    [Serializable]
    public abstract class Ailment : IntMod
    {
        protected Ailment(int modValue, string from, ModType modType) : base(modValue, from, modType)
        {
        }

        public abstract bool Gain(BaseCharacter character);
        public abstract bool Cure(BaseCharacter character);
    }
}
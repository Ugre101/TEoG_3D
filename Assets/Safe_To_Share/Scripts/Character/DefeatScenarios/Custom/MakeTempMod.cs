using System;
using Character.StatsStuff.Mods;

namespace Character.DefeatScenarios.Custom
{
    [Serializable]
    public class MakeTempMod
    {
        public int Duration = 1;
        public int Value;
        public ModType ModType = ModType.Flat;
    }
}
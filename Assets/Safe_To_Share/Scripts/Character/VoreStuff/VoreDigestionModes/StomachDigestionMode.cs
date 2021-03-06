using System;
using System.Collections.Generic;
using System.Linq;

namespace Character.VoreStuff.VoreDigestionModes
{
    [Serializable]
    public class StomachDigestionMode : VoreOrganDigestionMode
    {
        public override string[] AllDigestionTypes => new[] { Endo, Digestion, Absorption, };

        public override IEnumerable<string> GetPossibleDigestionTypes(BaseCharacter pred)
        {
            yield return Endo;
            yield return Digestion;
            if (pred.Vore.Level.OwnedPerks.OfType<VorePerks.Absorption>().Any())
                yield return Absorption;
        }

        public override void SetDigestionMode(int index)
        {
            CurrentModeID = index;
            digestionMethod = index switch
            {
                0 => new Endosoma(),
                1 => new DigestionOral(),
                2 => new Absorption(),
                _ => new Endosoma(),
            };
        }
    }
}
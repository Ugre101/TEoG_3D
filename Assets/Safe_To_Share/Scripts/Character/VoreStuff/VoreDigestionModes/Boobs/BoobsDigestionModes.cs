using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.VoreStuff.VorePerks;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Boobs
{
    [Serializable]
    public class BoobsDigestionModes : VoreOrganDigestionMode
    {
        public override string[] AllDigestionTypes => new[] { Endo, Digestion, Absorption, };

        public override IEnumerable<string> GetPossibleDigestionTypes(BaseCharacter pred)
        {
            yield return Endo;
            yield return Digestion;
            if (pred.Vore.Level.OwnedPerks.OfType<SexualOrganAbsorption>().Any())
                yield return Absorption;
        }

        public override void SetDigestionMode(int index)
        {
            CurrentModeID = index;
            digestionMethod = index switch
            {
                0 => new Endosoma(),
                1 => new MilkDigestion(),
                2 => new BoobsAbsorption(),
                _ => new Endosoma(),
            };
        }
    }
}
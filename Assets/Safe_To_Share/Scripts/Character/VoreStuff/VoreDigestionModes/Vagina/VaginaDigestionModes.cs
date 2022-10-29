using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.VoreStuff.VorePerks;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina
{
    [Serializable]
    public class VaginaDigestionModes : VoreOrganDigestionMode
    {
        public const string Rebirth = "Rebirth";
        public override string[] AllDigestionTypes => new[] { Endo, Digestion, Rebirth, };

        public override IEnumerable<string> GetPossibleDigestionTypes(BaseCharacter pred)
        {
            yield return Endo;
            yield return Digestion;
            if (pred.Vore.Level.OwnedPerks.OfType<ReBirth>().Any())
                yield return Rebirth;
        }

        public override void SetDigestionMode(int index)
        {
            CurrentModeID = index;
            digestionMethod = index switch
            {
                0 => new VaginaEndosoma(),
                1 => new DigestionVagina(),
                2 => new ReBirthMode(),
                _ => new Endosoma(),
            };
        }
    }
}
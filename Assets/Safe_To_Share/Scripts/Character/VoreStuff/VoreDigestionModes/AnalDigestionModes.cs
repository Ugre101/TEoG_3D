using System;
using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes
{
    [Serializable]
    public class AnalDigestionModes : VoreOrganDigestionMode
    {
        public override string[] AllDigestionTypes => new[] { Endo, Digestion, };

        public override void SetDigestionMode(int index)
        {
            CurrentModeID = index;
            digestionMethod = index switch
            {
                0 => new Endosoma(),
                1 => new DigestionAnal(),
                _ => new Endosoma(),
            };
        }
    }

    public class DigestionAnal : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) => true;
    }
}
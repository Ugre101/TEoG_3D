using System;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Balls {
    [Serializable]
    public class BallsDigestionModes : VoreOrganDigestionMode {
        public override string[] AllDigestionTypes => new[] { Endo, Digestion, };

        public override void SetDigestionMode(int index) {
            CurrentModeID = index;
            digestionMethod = index switch {
                0 => new EndosomaBalls(),
                1 => new DigestionBalls(),
                _ => new Endosoma(),
            };
        }
    }
}
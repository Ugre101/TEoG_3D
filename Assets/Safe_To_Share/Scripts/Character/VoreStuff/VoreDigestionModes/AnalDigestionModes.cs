using System;
using Character;
using Character.Organs;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Character.Scat;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes {
    [Serializable]
    public class AnalDigestionModes : VoreOrganDigestionMode {
        public override string[] AllDigestionTypes => new[] { Endo, Digestion, };

        public override void SetDigestionMode(int index) {
            CurrentModeID = index;
            digestionMethod = index switch {
                0 => new Endosoma(),
                1 => new DigestionAnal(),
                _ => new Endosoma(),
            };
        }
    }

    public sealed class DigestionAnal : DigestionMethod {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) {
            var digestTick = baseOrgan.Vore.DigestTick(
                pred.Vore.digestionStrength.Value / 3f,
                baseOrgan.Vore.Stretch, HandleAnalsDigestion,
                predIsPlayer);
            pred.SexualOrgans.Anals.Fluid.IncreaseCurrentValue(digestTick / 4f);
            var toGain = digestTick / 4f;
            var kcal = toGain / (pred.Body.Height.Value / 14);
            pred.Eat(Mathf.RoundToInt(kcal * 9000));
            return true;

            void HandleAnalsDigestion(Prey obj) {
                pred.OnOrganDigestion(SexualOrganType.Anal, obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
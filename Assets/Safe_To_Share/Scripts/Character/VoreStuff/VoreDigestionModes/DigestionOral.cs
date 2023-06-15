using Character;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Character.Scat;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes {
    public sealed class DigestionOral : DigestionMethod {
        public override bool Tick(BaseCharacter pred, VoreOrgan voreOrgan, bool predIsPlayer) {
            var toGain = voreOrgan.DigestTick(pred.Vore.digestionStrength.Value, voreOrgan.Stretch,
                HandleStomachDigestion, predIsPlayer);
            var kcal = toGain / (pred.Body.Height.Value / 14);
            pred.Eat(Mathf.RoundToInt(kcal * 9000));
            return true;

            void HandleStomachDigestion(Prey obj) {
                pred.OnStomachDigestion(obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
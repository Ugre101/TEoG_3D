using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Balls {
    public sealed class DigestionBalls : DigestionMethod {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) {
            pred.SexualOrgans.Balls.Fluid.IncreaseCurrentValue(baseOrgan.Vore.DigestTick(
                                                                   pred.Vore.digestionStrength.Value / 3f,
                                                                   baseOrgan.Vore.Stretch, HandleBallsDigestion,
                                                                   predIsPlayer) /
                                                               2f);
            return true;

            void HandleBallsDigestion(Prey obj) {
                pred.OnOrganDigestion(SexualOrganType.Balls, obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
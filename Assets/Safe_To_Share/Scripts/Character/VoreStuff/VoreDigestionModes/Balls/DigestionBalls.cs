using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes.Balls
{
    public class DigestionBalls : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            pred.SexualOrgans.Balls.Fluid.IncreaseCurrentValue(baseOrgan.Vore.DigestTick(
                                                                   pred.Vore.digestionStrength.Value / 3f,
                                                                   baseOrgan.Vore.Stretch, HandleBallsDigestion,
                                                                   predIsPlayer) /
                                                               2f);
            return true;

            void HandleBallsDigestion(Prey obj)
            {
                pred.OnOrganDigestion(SexualOrganType.Balls, obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
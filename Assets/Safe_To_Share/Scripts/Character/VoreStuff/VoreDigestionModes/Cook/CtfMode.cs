using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes.Cook
{
    public class CtfMode : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VoreOrganDigestionMode.Absorption, baseOrgan.Vore, VoreType.Cock);
            return false;
        }
    }
}
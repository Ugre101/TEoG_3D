using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes.Boobs
{
    public class BoobsAbsorption : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VoreOrganDigestionMode.Absorption, baseOrgan.Vore, VoreType.Breast);
            return true;
        }
    }
}
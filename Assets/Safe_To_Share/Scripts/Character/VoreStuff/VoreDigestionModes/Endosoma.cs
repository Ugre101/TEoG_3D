using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes
{
    public class Endosoma : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            return false;
        }

        public override bool Tick(BaseCharacter pred, VoreOrgan voreOrgan, bool predIsPlayer)
        {
            voreOrgan.NoDigestTick();
            return false;
        }
    }
}
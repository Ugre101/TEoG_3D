using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes
{
    public class Absorption : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VoreOrganDigestionMode.Absorption, baseOrgan.Vore, VoreType.Oral);
            return false;
            //      AltDigestionPerk(pred, VoreDigestionMode.Absorption, Stomach, VoreType.Oral);
        }

        public override bool Tick(BaseCharacter pred, VoreOrgan voreOrgan, bool predIsPlayer)
        {
            voreOrgan.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VoreOrganDigestionMode.Absorption, voreOrgan, VoreType.Oral);
            return false;
            //      AltDigestionPerk(pred, VoreDigestionMode.Absorption, Stomach, VoreType.Oral);
        }
    }
}
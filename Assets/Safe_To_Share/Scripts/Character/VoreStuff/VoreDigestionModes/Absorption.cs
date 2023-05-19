using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes
{
    public sealed class Absorption : DigestionMethod
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
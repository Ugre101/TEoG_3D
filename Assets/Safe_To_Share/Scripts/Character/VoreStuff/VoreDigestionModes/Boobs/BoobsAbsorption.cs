using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Boobs
{
    public sealed class BoobsAbsorption : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VoreOrganDigestionMode.Absorption, baseOrgan.Vore, VoreType.Breast);
            return true;
        }
    }
}
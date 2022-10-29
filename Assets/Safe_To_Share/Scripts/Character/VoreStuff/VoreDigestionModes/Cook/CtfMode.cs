using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Cook
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
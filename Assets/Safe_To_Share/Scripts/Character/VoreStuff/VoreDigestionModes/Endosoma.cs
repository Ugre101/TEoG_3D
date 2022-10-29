using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes
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
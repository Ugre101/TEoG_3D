using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina {
    public sealed class ReBirthMode : DigestionMethod {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) {
            baseOrgan.Vore.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VaginaDigestionModes.Rebirth, baseOrgan.Vore, VoreType.UnBirth);
            return false;
        }
    }
}
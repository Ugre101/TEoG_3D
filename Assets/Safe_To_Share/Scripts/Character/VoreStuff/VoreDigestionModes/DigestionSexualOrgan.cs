using Character;
using Character.Organs;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes {
    public class DigestionSexualOrgan : DigestionMethod {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) => true;
    }
}
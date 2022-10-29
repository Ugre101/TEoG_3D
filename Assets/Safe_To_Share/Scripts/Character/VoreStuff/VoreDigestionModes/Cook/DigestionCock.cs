using Character;
using Character.Organs;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Cook
{
    public class DigestionCock : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) => false;
    }
}
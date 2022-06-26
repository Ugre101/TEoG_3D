using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes.Cook
{
    public class DigestionCock : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            return false;
        }
    }
}
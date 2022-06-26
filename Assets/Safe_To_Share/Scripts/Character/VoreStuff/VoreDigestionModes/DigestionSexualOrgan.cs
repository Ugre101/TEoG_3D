using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes
{
    public class DigestionSexualOrgan : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            return true;
        }
    }
}
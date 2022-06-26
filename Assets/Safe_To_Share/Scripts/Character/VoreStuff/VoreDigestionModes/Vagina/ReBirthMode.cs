using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes.Vagina
{
    public class ReBirthMode : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            pred.Vore.AltDigestionPerk(pred, VaginaDigestionModes.Rebirth, baseOrgan.Vore, VoreType.UnBirth);
            return false;
        }
    }
}
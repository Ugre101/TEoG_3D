namespace Character.VoreStuff.VoreDigestionModes
{
    public class DigestionOral : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, VoreOrgan voreOrgan, bool predIsPlayer)
        {
            float toGain = voreOrgan.DigestTick(pred.Vore.digestionStrength.Value, voreOrgan.Stretch, HandleStomachDigestion, predIsPlayer);
            pred.Body.Fat.BaseValue += toGain / (pred.Body.Height.Value / 14); // 50%
            return true;

            void HandleStomachDigestion(Prey obj)
            {
                pred.OnStomachDigestion(obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
namespace Character.VoreStuff.VoreDigestionModes
{
    public class DigestionOral : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, VoreOrgan voreOrgan, bool predIsPlayer)
        {
            pred.Body.Fat.BaseValue += voreOrgan.DigestTick(pred.Vore.digestionStrength.Value, voreOrgan.Stretch,
                HandleStomachDigestion, predIsPlayer) / 5f; // 50%
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
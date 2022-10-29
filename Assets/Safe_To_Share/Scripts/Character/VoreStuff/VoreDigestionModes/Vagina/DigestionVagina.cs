using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina
{
    public class DigestionVagina : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            pred.SexualOrgans.Vaginas.Fluid.IncreaseCurrentValue(
                baseOrgan.Vore.DigestTick(pred.Vore.digestionStrength.Value / 3f, baseOrgan.Vore.Stretch,
                    HandleVaginaDigestion, predIsPlayer) /
                2f);
            return true;

            void HandleVaginaDigestion(Prey obj)
            {
                pred.OnOrganDigestion(SexualOrganType.Vagina, obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
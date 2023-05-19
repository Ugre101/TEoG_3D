using Character;
using Character.Organs;
using Character.PregnancyStuff;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina
{
    public sealed class VaginaEndosoma : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            return false;
        }


        public override void OnPreyOrgasmInSexualOrgan(BaseCharacter pred, BaseOrgan baseOrgan, Prey prey, int orgasms)
        {
            if (baseOrgan.Womb.HasFetus) return;
            if (!prey.SexualOrgans.Balls.HaveAny() || !prey.SexualOrgans.Dicks.HaveAny()) return;
            for (int i = 0; i < orgasms; i++)
                if (pred.TryImpregnate(prey, baseOrgan))
                    break;
        }
    }
}
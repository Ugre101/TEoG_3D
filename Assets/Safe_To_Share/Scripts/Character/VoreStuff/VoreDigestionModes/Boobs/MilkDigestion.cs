using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Boobs
{
    public class MilkDigestion : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            pred.SexualOrgans.Boobs.Fluid.IncreaseCurrentValue(baseOrgan.Vore.DigestTick(
                                                                   pred.Vore.digestionStrength.Value / 3f,
                                                                   baseOrgan.Vore.Stretch, HandleBoobsDigestion,
                                                                   predIsPlayer) /
                                                               2f);
            return true;

            void HandleBoobsDigestion(Prey obj)
            {
                pred.OnOrganDigestion(SexualOrganType.Boobs, obj, VoreOrganDigestionMode.Digestion);
                VoreSystem.HaveDigested(obj.Identity.ID);
                VoredCharacters.RemovePrey(obj);
            }
        }
    }
}
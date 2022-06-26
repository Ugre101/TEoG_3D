using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes.Boobs
{
    public class MilkDigestion: DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            pred.SexualOrgans.Boobs.Fluid.IncreaseCurrentValue(baseOrgan.Vore.DigestTick(pred.Vore.digestionStrength.Value / 3f, baseOrgan.Vore.Stretch, HandleBoobsDigestion,predIsPlayer) / 2f);
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
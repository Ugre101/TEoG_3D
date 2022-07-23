using Character.Organs;

namespace Character.VoreStuff.VoreDigestionModes
{
    public abstract class DigestionMethod
    {
        public virtual bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer) => false;

        public virtual bool Tick(BaseCharacter pred, VoreOrgan voreOrgan, bool predIsPlayer) => false;

        public virtual void OnPreyOrgasm(Prey value, int orgasms)
        {
        }

        public virtual void OnPreyOrgasmInSexualOrgan(BaseCharacter pred, BaseOrgan baseOrgan, Prey prey, int orgasms)
        {
        }
    }
}
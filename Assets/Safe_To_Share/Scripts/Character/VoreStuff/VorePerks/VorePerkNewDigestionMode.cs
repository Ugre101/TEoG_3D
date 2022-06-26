using System.Collections.Generic;
using Character.Organs;

namespace Character.VoreStuff.VorePerks
{
    public abstract class VorePerkNewDigestionMode : VorePerk
    {
        public abstract string  DigestionMode { get; }

        public virtual void OnDigestionTick(BaseCharacter pred, VoreOrgan voreOrgan, VoreType voreType)
        {
            
        }
        public virtual void OnOrganDigestionTick(BaseCharacter pred, BaseOrgan baseOrgan, VoreType voreType)
        {

        }

        public virtual void SpecialOrganDigestion(BaseCharacter pred, BaseOrgan organ, SexualOrganType voreType,
            int prey, bool predIsPlayer)
        {
            
        }
        public abstract IEnumerable<VoreType> OrganType { get; }
    }
}
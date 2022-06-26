using System.Collections.Generic;
using System.Linq;
using Character.Organs;
using Character.VoreStuff.VoreDigestionModes;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.VoreStuff.VorePerks
{
    [CreateAssetMenu(fileName = "SexualOrganAbsorption", menuName = "Character/Vore/SexualOrganAbsorption", order = 0)]
    public class SexualOrganAbsorption : VorePerkNewPredationMode
    {
        const float AbsorbSpeedMod = 1f;

        public override string DigestionMode => VoreOrganDigestionMode.Absorption;


        public override IEnumerable<VoreType> OrganType => new[] { VoreType.Cock, VoreType.Breast,};


        public override void SpecialOrganDigestion(BaseCharacter pred, BaseOrgan organ, SexualOrganType type, int prey,
            bool predIsPlayer) => AbsorbPrey(pred,organ,prey,type);

        static void AbsorbPrey(BaseCharacter pred, BaseOrgan baseOrgan, int preyId, SexualOrganType type)
        {
            if (!VoredCharacters.PreyDict.TryGetValue(preyId, out Prey prey))
                return;
            if (prey.AltProgress >= 100f)
            {
                FullyAbsorbed(pred, baseOrgan, prey, preyId, type);
                return;
            }
            MorphToOrgan(prey, baseOrgan);
        }

        static void MorphToOrgan(Prey prey, BaseIntStat baseOrgan)
        {
            float diff = MorphTickAmount(prey, baseOrgan);
            prey.AltDigest(diff);
        }

        static float MorphTickAmount(BaseCharacter prey, BaseIntStat baseOrgan) => baseOrgan.Value / prey.Body.Weight * AbsorbSpeedMod;

        public static bool CanInstaMorph(BaseCharacter prey, BaseIntStat baseOrgan) =>
            MorphTickAmount(prey, baseOrgan) >= 100f;
        static void FullyAbsorbed(BaseCharacter pred,BaseOrgan organ, Prey prey, int preyId, SexualOrganType type)
        {
            organ.Vore.RemovePrey(preyId);
            VoredCharacters.RemovePrey(prey);
            AddToOrgan(pred, organ, prey, type);
            // EventLog.AddEvent($"{pred.Identity.FirstName} has fully absorbed {prey.Identity.FullName}");
        }

        public static void AddToOrgan(BaseCharacter pred, BaseOrgan organ, Prey prey, SexualOrganType type)
        {
            pred.OnOrganDigestion(type, prey, VoreOrganDigestionMode.Absorption);
            float diff = prey.Body.Weight / organ.BaseValue;
            int growBy = Mathf.RoundToInt(Mathf.Sqrt(diff)); // need to expand futher
            if (growBy > 0)
                organ.BaseValue += growBy;
        }
    }
}
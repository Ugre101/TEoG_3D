using System.Collections.Generic;
using System.Linq;
using Character.BodyStuff;
using Character.Organs;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina;
using UnityEngine;

namespace Character.VoreStuff.VorePerks
{
    [CreateAssetMenu(fileName = "Rebirth perk", menuName = "Character/Vore/Vore Rebirth", order = 0)]
    public sealed class ReBirth : VorePerkNewDigestionMode
    {
        public override string DigestionMode => VaginaDigestionModes.Rebirth;

        public override IEnumerable<VoreType> OrganType => new[] { VoreType.UnBirth, };

        public override void OnDigestionTick(BaseCharacter pred, VoreOrgan voreOrgan, VoreType type)
        {
            for (int index = voreOrgan.PreysIds.Count; index-- > 0;)
                ReBirthPrey(pred, voreOrgan, index);
        }

        static void ReBirthPrey(BaseCharacter pred, VoreOrgan voreOrgan, int index)
        {
            int preysId = voreOrgan.PreysIds[index];
            if (!VoredCharacters.PreyDict.TryGetValue(preysId, out Prey prey))
                return;
            float altDigest = prey.AltDigest();
            if (altDigest < 100f)
                prey.Body.ShrinkBody(altDigest / 1000f);
            else
                TurnToFetus(pred, voreOrgan, preysId, prey);
        }

        static void TurnToFetus(BaseCharacter pred, VoreOrgan voreOrgan, int preysId, Prey prey)
        {
            var baseOrgan = pred.SexualOrgans.Vaginas.BaseList.FirstOrDefault(v => v.Vore.PreysIds.Contains(preysId));
            baseOrgan?.Womb.AddFetus(pred, prey);
            pred.OnOrganDigestion(SexualOrganType.Vagina, prey, VaginaDigestionModes.Rebirth);
            voreOrgan.RemovePrey(preysId);
            VoredCharacters.RemovePrey(prey);
        }
    }
}
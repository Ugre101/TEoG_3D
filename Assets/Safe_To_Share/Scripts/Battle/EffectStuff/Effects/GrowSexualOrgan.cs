using System;
using Character;
using Character.Organs;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects {
    [Serializable]
    public class GrowSexualOrgan : Effect {
        [SerializeField] bool growAll;
        [SerializeField] SexualOrganType organType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            if (!target.SexualOrgans.Containers.TryGetValue(organType, out var organ)) return;
            if (!organ.HaveAny())
                return;
            if (growAll) {
                foreach (var baseOrgan in organ.BaseList)
                    baseOrgan.BaseValue += FinalIntValue(user, baseOrgan.BaseValue);
            } else {
                var randomOrgan = organ.GetRandomOrgan();
                if (randomOrgan != null)
                    randomOrgan.BaseValue += FinalIntValue(user, randomOrgan.BaseValue);
            }
        }
    }
}
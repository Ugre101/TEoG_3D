using System;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects
{
    [Serializable]
    public class ShrinkSexualOrgan : Effect
    {
        [SerializeField] bool shrinkAll;
        [SerializeField] SexualOrganType organType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            if (!target.SexualOrgans.Containers.TryGetValue(organType, out var organ)) return;
            if (!organ.HaveAny())
                return;
            if (shrinkAll)
                foreach (var baseOrgan in organ.BaseList)
                    baseOrgan.BaseValue -= FinalIntValue(user, baseOrgan.BaseValue);
            else
            {
                var elementAtOrDefault = organ.GetRandomOrgan();
                if (elementAtOrDefault != null)
                    elementAtOrDefault.BaseValue -= FinalIntValue(user, elementAtOrDefault.BaseValue);
            }
        }
    }
}
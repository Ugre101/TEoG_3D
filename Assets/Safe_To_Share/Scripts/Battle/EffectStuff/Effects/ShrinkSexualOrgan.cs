using System;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Battle.EffectStuff.Effects
{
    [Serializable]
    public class ShrinkSexualOrgan : Effect
    {
        [SerializeField] bool shrinkAll;
        [SerializeField] SexualOrganType organType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            if (!target.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer organ)) return;
            if (!organ.HaveAny())
                return;
            if (shrinkAll)
                foreach (BaseOrgan baseOrgan in organ.List)
                    baseOrgan.BaseValue -= FinalIntValue(user, baseOrgan.BaseValue);
            else
            {
                BaseOrgan elementAtOrDefault = organ.GetRandomOrgan();
                if (elementAtOrDefault != null)
                    elementAtOrDefault.BaseValue -= FinalIntValue(user, elementAtOrDefault.BaseValue);
            }
        }
    }
}
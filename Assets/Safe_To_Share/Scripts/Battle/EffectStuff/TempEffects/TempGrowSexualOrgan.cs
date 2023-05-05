using System;
using Battle.EffectStuff.Effects;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Battle.EffectStuff.TempEffects
{
    [Serializable]
    public class TempGrowSexualOrgan : TempEffect
    {
        [SerializeField] bool growAll;
        [SerializeField] SexualOrganType organType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            if (target.SexualOrgans.Containers.TryGetValue(organType, out BaseOrgansContainer organ))
            {
                if (!organ.HaveAny())
                    return;
                if (growAll)
                    foreach (BaseOrgan baseOrgan in organ.BaseList)
                        baseOrgan.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowSexualOrgan)));
                else
                {
                    BaseOrgan elementAtOrDefault = organ.GetRandomOrgan();
                    elementAtOrDefault?.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowSexualOrgan)));
                }
            }
        }
    }
}
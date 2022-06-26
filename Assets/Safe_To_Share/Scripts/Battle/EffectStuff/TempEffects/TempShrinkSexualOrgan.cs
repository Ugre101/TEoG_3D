using System;
using Battle.EffectStuff.Effects;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Battle.EffectStuff.TempEffects
{
    [Serializable]
    public class TempShrinkSexualOrgan : TempEffect
    {
        [SerializeField] bool shrinkAll;
        [SerializeField] SexualOrganType organType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target)
        {
            if (target.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer organ))
            {
                if (!organ.HaveAny())
                    return;
                if (shrinkAll)
                    foreach (BaseOrgan baseOrgan in organ.List)
                        baseOrgan.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowSexualOrgan), true));
                else
                {
                    BaseOrgan elementAtOrDefault = organ.GetRandomOrgan();
                    elementAtOrDefault?.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowSexualOrgan), true));
                }
            }
        }
    }
}
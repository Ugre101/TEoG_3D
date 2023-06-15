using System;
using Character;
using Character.Organs;
using Safe_To_Share.Scripts.Battle.EffectStuff.Effects;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.TempEffects {
    [Serializable]
    public class TempShrinkSexualOrgan : TempEffect {
        [SerializeField] bool shrinkAll;
        [SerializeField] SexualOrganType organType;

        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            if (!target.SexualOrgans.Containers.TryGetValue(organType, out var organ)) return;
            if (!organ.HaveAny())
                return;
            if (shrinkAll) {
                foreach (var baseOrgan in organ.BaseList)
                    baseOrgan.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowSexualOrgan), true));
            } else {
                var randomOrgan = organ.GetRandomOrgan();
                randomOrgan?.Mods.AddTempStatMod(TempIntMod(user, nameof(GrowSexualOrgan), true));
            }
        }
    }
}
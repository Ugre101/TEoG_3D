using System;
using System.Collections;
using Battle.CombatantStuff;
using UnityEngine;

namespace Battle
{
    [Serializable]
    public class 
        BattleAct
    {
        [SerializeField] BattlePrefabEffect onCasterEffect;
        [SerializeField] bool instancePrefabOnCasterFirst;
        [SerializeField] BattlePrefabEffect onTargetEffect;
        [SerializeField] bool instancePrefabOnTargetFirst;

        [Range(0, 5f), SerializeField,] float instanceCastDelay = 0.2f;

        public IEnumerator InstanceEffects(Combatant caster, Combatant target)
        {
            if (instancePrefabOnCasterFirst)
            {
                CastIfNotNull(caster, onCasterEffect);
                yield return new WaitForSeconds(instanceCastDelay);
                CastIfNotNull(target, onTargetEffect);
            }
            else if (instancePrefabOnTargetFirst)
            {
                CastIfNotNull(target, onTargetEffect);
                yield return new WaitForSeconds(instanceCastDelay);
                CastIfNotNull(caster, onCasterEffect);
            }

            CastIfNotNull(target, onTargetEffect);
            CastIfNotNull(caster, onCasterEffect);
            float longestWait = onCasterEffect.StayTime < onTargetEffect.StayTime
                ? onTargetEffect.StayTime
                : onCasterEffect.StayTime;
            yield return new WaitForSeconds(longestWait);

            static void CastIfNotNull(Combatant castOn, BattlePrefabEffect prefab)
            {
                if (prefab.Prefab != null)
                    castOn.CastPrefabOn(prefab);
                if (prefab.TriggerAnimation != TriggerBattleAnimations.None)
                    castOn.TriggerAnimation(prefab.TriggerAnimation);
            }
        }
    }

    [Serializable]
    public struct BattlePrefabEffect
    {
        [SerializeField] GameObject prefab;
        [SerializeField] TriggerBattleAnimations triggerBattleAnimations;
        [SerializeField] FloatBattleAnimations floatBattleAnimations;
        [SerializeField] float stayTime;

        public GameObject Prefab => prefab;
        public float StayTime => stayTime;

        public TriggerBattleAnimations TriggerAnimation => triggerBattleAnimations;

        public FloatBattleAnimations FloatAnimation => floatBattleAnimations;
    }
}
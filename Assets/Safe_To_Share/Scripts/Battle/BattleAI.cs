using System;
using System.Collections;
using Battle.SkillsAndSpells;
using Character.StatsStuff;
using Character.StatsStuff.HealthStuff;
using CustomClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace Battle
{
    [CreateAssetMenu(menuName = "Create BattleAI", fileName = "BattleAI", order = 0)]
    public class BattleAI : ScriptableObject
    {
        [SerializeField,] DropSerializableObject hitStandardGuid;
        [SerializeField,] DropSerializableObject teaseStandardGuid;
        [SerializeField,] DropSerializableObject rareUseGuid;
        [SerializeField,] DropSerializableObject lowHpGuid;
        [SerializeField,] DropSerializableObject lowWpGuid;
        [SerializeField] float rareCastChance = 0.1f;
        Ability castOnSelfWhenLowHealth;
        Ability castOnSelfWhenLowWillHealth;
        Ability hitStandardAbility;
        Ability rareUseAbility;

        bool standardIsNotNull, teaseIsNotNull, rareIsNotNull, hpLowIsNotNull, wpLowIsNotNull;

        Ability teaseStandardAbility;
/*
        public IEnumerable<AsyncOperationHandle<Ability>> LoadBattleUI()
        {
            yield return Load(new BattleAISave(standardGuid, rareUseGuid, lowHpGuid, lowWpGuid));
        }
*/
        bool loaded;


       public void SetLoadedFalse() => loaded = false;
        
        public IEnumerator HandleTurn(CombatCharacter caster, CombatCharacter target)
        {
            if (!loaded)
                yield return Load();
            
            Stats casterStats = caster.Character.Stats;
            if (CastHealthAbility(hpLowIsNotNull, casterStats.Health))
                yield return castOnSelfWhenLowHealth.UseEffect(caster, caster);

            if (CastHealthAbility(wpLowIsNotNull, casterStats.WillPower))
                yield return castOnSelfWhenLowWillHealth.UseEffect(caster, caster);

            if (rareIsNotNull && Random.value <= rareCastChance)
                yield return rareUseAbility.UseEffect(caster, target);
            else if (standardIsNotNull && teaseIsNotNull)
            {
                if (caster.Character.Stats.Strength.Value > caster.Character.Stats.Charisma.Value)
                    yield return hitStandardAbility.UseEffect(caster, target);
                else
                    yield return teaseStandardAbility.UseEffect(caster, target);
            }
            else
                Debug.LogWarning("Battle ai has no attacks");

            yield return null;

            static bool CastHealthAbility(bool notNull, RecoveryIntStat health) =>
                notNull && (float)health.CurrentValue / health.Value <= 0.33f;
        }

        IEnumerator Load()
        {
            loaded = true;
            if (!string.IsNullOrEmpty(hitStandardGuid.guid))
            {
                AsyncOperationHandle<Ability> stanOp = Addressables.LoadAssetAsync<Ability>(hitStandardGuid.guid);
                yield return stanOp;
                if (stanOp.Status == AsyncOperationStatus.Succeeded)
                {
                    hitStandardAbility = stanOp.Result;
                    standardIsNotNull = true;
                }
            }

            if (!string.IsNullOrEmpty(teaseStandardGuid.guid))
            {
                AsyncOperationHandle<Ability> stanOp = Addressables.LoadAssetAsync<Ability>(teaseStandardGuid.guid);
                yield return stanOp;
                if (stanOp.Status == AsyncOperationStatus.Succeeded)
                {
                    teaseStandardAbility = stanOp.Result;
                    teaseIsNotNull = true;
                }
            }

            if (!string.IsNullOrEmpty(rareUseGuid.guid))
            {
                var rareOp = Addressables.LoadAssetAsync<Ability>(rareUseGuid.guid);
                yield return rareOp;
                if (rareOp.Status == AsyncOperationStatus.Succeeded)
                {
                    rareUseAbility = rareOp.Result;
                    rareIsNotNull = true;
                }
            }

            if (!string.IsNullOrEmpty(lowHpGuid.guid))
            {
                var hpOp = Addressables.LoadAssetAsync<Ability>(lowHpGuid.guid);
                yield return hpOp;
                if (hpOp.Status == AsyncOperationStatus.Succeeded)
                {
                    castOnSelfWhenLowHealth = hpOp.Result;
                    hpLowIsNotNull = true;
                }
            }

            if (!string.IsNullOrEmpty(lowWpGuid.guid))
            {
                var wpOp = Addressables.LoadAssetAsync<Ability>(lowWpGuid.guid);
                yield return wpOp;
                if (wpOp.Status == AsyncOperationStatus.Succeeded)
                {
                    castOnSelfWhenLowWillHealth = wpOp.Result;
                    wpLowIsNotNull = true;
                }
            }
        }
    }
}
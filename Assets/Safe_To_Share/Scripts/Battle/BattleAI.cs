using System.Collections;
using System.Collections.Generic;
using Battle.SkillsAndSpells;
using Character.StatsStuff.HealthStuff;
using CustomClasses;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Battle
{
    [CreateAssetMenu(menuName = "Create BattleAI", fileName = "BattleAI", order = 0)]
    public sealed class BattleAI : ScriptableObject
    {
        [SerializeField,] DropSerializableObject<Skill> hitStandardGuid;
        [SerializeField,] DropSerializableObject<Ability> teaseStandardGuid;
        [SerializeField,] DropSerializableObject<Ability> rareUseGuid;
        [SerializeField,] DropSerializableObject<Ability> lowHpGuid;
        [SerializeField,] DropSerializableObject<Ability> lowWpGuid;
        [SerializeField] float rareCastChance = 0.1f;


        bool firstUse = true;

        readonly LoadedAbility standard = new(),tease = new(),
          rare = new(),hpLow = new(), wpLow = new();


        public IEnumerator HandleTurn(CombatCharacter caster, CombatCharacter target)
        {
            if (firstUse)
                yield return Load();

            var casterStats = caster.Character.Stats;
            if (CastHealthAbility(hpLow.NotNull, casterStats.Health))
                yield return hpLow.Ability.UseEffect(caster, caster);

            if (CastHealthAbility(wpLow.NotNull, casterStats.WillPower))
                yield return wpLow.Ability.UseEffect(caster, caster);

            if (rare.NotNull && Random.value <= rareCastChance)
            {
                yield return rare.Ability.UseEffect(caster, target);
            }
            else if (standard.NotNull && tease.NotNull)
            {
                if (caster.Character.Stats.Strength.Value > caster.Character.Stats.Charisma.Value)
                    yield return standard.Ability.UseEffect(caster, target);
                else
                    yield return tease.Ability.UseEffect(caster, target);
            }
            else
            {
                Debug.LogWarning("Battle ai has no attacks");
            }

            yield return null;

            static bool CastHealthAbility(bool notNull, RecoveryIntStat health) =>
                notNull && (float)health.CurrentValue / health.Value <= 0.33f;
        }

        IEnumerator Load()
        {
            firstUse = false;
            var load = new List<IEnumerator>
            {
                standard.Load(hitStandardGuid.guid),
                tease.Load(teaseStandardGuid.guid),
                rare.Load(rareUseGuid.guid),
                hpLow.Load(lowHpGuid.guid),
                wpLow.Load(lowWpGuid.guid),
            };
            foreach (var enumerator in load)
                yield return enumerator;
        }

        public void CleanUp()
        {
            firstUse = true;
            standard.Clean();
            tease.Clean();
            rare.Clean();
            hpLow.Clean();
            wpLow.Clean();
        }

        sealed class LoadedAbility
        {
            public bool NotNull;
            public Ability Ability;

            public void Clean()
            {
                if (NotNull)
                    Addressables.Release(Ability);
                NotNull = false;
            }
            public IEnumerator Load(string guid)
            {
                if (string.IsNullOrWhiteSpace(guid))
                    yield break;
                var op = Addressables.LoadAssetAsync<Ability>(guid);
                yield return op;
                if (op.Status is not AsyncOperationStatus.Succeeded) 
                    yield break;
                NotNull = true;
                Ability = op.Result;
            }
        }
    }
}
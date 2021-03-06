using System;
using System.Collections;
using System.Collections.Generic;
using Battle.EffectStuff;
using Character;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Battle.SkillsAndSpells
{
    public abstract class Ability : SObjSavableTitleDescIcon
    {
        [SerializeField] protected bool canTargetEnemies = true, canTargetAllies;
        [SerializeField] protected List<UseCost> useCosts = new();
        [SerializeField] protected EffectsTree effectsTree = new();
        [SerializeField] BattleAct battleAct;
        public EffectsTree EffectsTree => effectsTree;
        public IEnumerable<UseCost> UseCosts => useCosts;

        protected bool CanTargetEnemies => canTargetEnemies;

        protected bool CanTargetAllies => canTargetAllies;

        public virtual IEnumerator UseEffect(CombatCharacter user, CombatCharacter[] targets)
        {
            HandleUseCost(user.Character);
            foreach (CombatCharacter target in targets)
            {
                foreach (Effect effectsTreeActiveEffect in EffectsTree.ActiveEffects)
                    effectsTreeActiveEffect.UseEffect(user.Character, target.Character);
                yield return battleAct.InstanceEffects(user.Combatant, target.Combatant);
            }
        }


        public virtual IEnumerator UseEffect(CombatCharacter user, CombatCharacter target)
        {
            foreach (Effect effectsTreeActiveEffect in EffectsTree.ActiveEffects)
                effectsTreeActiveEffect.UseEffect(user.Character, target.Character);
            HandleUseCost(user.Character);
            yield return battleAct.InstanceEffects(user.Combatant, target.Combatant);
        }

        void HandleUseCost(BaseCharacter user)
        {
            foreach (UseCost cost in UseCosts)
                switch (cost.Type)
                {
                    case UseCost.CostType.Stamina: break;
                    case UseCost.CostType.Mana: break;
                    case UseCost.CostType.Health:
                        user.Stats.Health.DecreaseCurrentValue(cost.Cost);
                        break;
                    case UseCost.CostType.WillPower:
                        user.Stats.WillPower.DecreaseCurrentValue(cost.Cost);
                        break;
                    case UseCost.CostType.Gold: break;
                    default: throw new ArgumentOutOfRangeException();
                }
        }

        public virtual bool CanUse(BaseCharacter user, BaseCharacter target)
        {
            foreach (UseCost cost in UseCosts)
                switch (cost.Type)
                {
                    case UseCost.CostType.Stamina:
                        break;
                    case UseCost.CostType.Mana:
                        break;
                    case UseCost.CostType.Health:
                        if (user.Stats.Health.Value <= cost.Cost) return false;
                        break;
                    case UseCost.CostType.WillPower:
                        if (user.Stats.WillPower.Value <= cost.Cost) return false;
                        break;
                    case UseCost.CostType.Gold:
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }

            return true;
        }
    }
}
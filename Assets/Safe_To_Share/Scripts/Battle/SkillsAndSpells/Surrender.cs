using System.Collections;
using UnityEngine;

namespace Battle.SkillsAndSpells
{
    [CreateAssetMenu(menuName = "Character/Ability/Surrender", fileName = "Surrender", order = 0)]
    public class Surrender : Ability
    {
        public override IEnumerator UseEffect(CombatCharacter user, CombatCharacter target)
        {
            BattleManager.Instance.GoToDefeat();
            return base.UseEffect(user, target);
        }
    }
}
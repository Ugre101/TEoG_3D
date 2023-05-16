using System;
using System.Linq;
using Safe_To_Share.Scripts.Battle.CombatantStuff;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle
{
    public sealed class BattleTarget : MonoBehaviour
    {
        [SerializeField] LayerMask enemyIsOnLayer;

        int enemyTargetIndex;
        CombatCharacter[] possibleEnemyTargets;

        public CombatCharacter EnemyTargeted
        {
            get
            {
                if (enemyTargetIndex > possibleEnemyTargets.Length - 1)
                    enemyTargetIndex = 0;
                return possibleEnemyTargets[enemyTargetIndex];
            }
        }

        public void SetPossibleTargets(CombatCharacter[] combatCharacters) => possibleEnemyTargets = combatCharacters;

        public void SwitchTargetLeft(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                ShiftTarget(enemyTargetIndex < possibleEnemyTargets.Length - 1 ? enemyTargetIndex + 1 : 0);
        }

        public void SwitchTargetRight(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                ShiftTarget(enemyTargetIndex > 0 ? enemyTargetIndex - 1 : possibleEnemyTargets.Length - 1);
        }

        public void ClickTarget(InputAction.CallbackContext ctx)
        {
            var mousePos = Pointer.current.position.ReadValue();
            if (Camera.main is not { } cam) return;
            var ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, enemyIsOnLayer) &&
                hit.transform.gameObject.TryGetComponent(out Combatant combatant))
                MatchTarget(combatant);
        }

        void MatchTarget(Combatant combatant)
        {
            foreach (var possibleTarget in possibleEnemyTargets)
                if (possibleTarget.Combatant == combatant)
                {
                    ShiftTarget(Array.IndexOf(possibleEnemyTargets,possibleTarget));
                    return;
                }
        }

        void ShiftTarget(int newIndex)
        {
            EnemyTargeted.Combatant.StopTargeting();
            if (possibleEnemyTargets.Length <= 1)
                enemyTargetIndex = 0;
            enemyTargetIndex = newIndex;
            EnemyTargeted.Combatant.Target();
        }
    }
}
using System;
using System.Linq;
using Battle.CombatantStuff;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle
{
    public class BattleTarget : MonoBehaviour
    {
        
        int enemyTargetIndex;
        CombatCharacter[] possibleEnemyTargets;
        [SerializeField] LayerMask enemyIsOnLayer;

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
            Vector2 mousePos = Pointer.current.position.ReadValue();
            if (Camera.main is not { } cam) return;
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, enemyIsOnLayer) && hit.transform.gameObject.TryGetComponent(out Combatant combatant))
                MatchTarget(combatant);
        }

        void MatchTarget(Combatant component)
        {
            CombatCharacter target = possibleEnemyTargets?.FirstOrDefault(et => et.Combatant.Equals(component));
            if (target != null)
                ShiftTarget(Array.IndexOf(possibleEnemyTargets, target));
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
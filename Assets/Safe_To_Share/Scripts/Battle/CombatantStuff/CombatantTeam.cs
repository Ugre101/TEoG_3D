using System.Linq;
using Character;
using UnityEngine;

namespace Battle.CombatantStuff
{
    public class CombatantTeam : MonoBehaviour
    {
        [SerializeField] CombatantSlot[] slots;

#if  UNITY_EDITOR
        void OnValidate() => slots = GetComponentsInChildren<CombatantSlot>();
#endif

        public void FirstSetup()
        {
            foreach (CombatantSlot slot in slots)
                slot.EmptySlot();
        }

        public Combatant SetupTeam(BaseCharacter obj)
        {
            CombatantSlot emptySlot = slots.FirstOrDefault(cs => cs.Empty);
            return emptySlot != null ? emptySlot.AddCombatant(obj) : null;
        }
    }
}
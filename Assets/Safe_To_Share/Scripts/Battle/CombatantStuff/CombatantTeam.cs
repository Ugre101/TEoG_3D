using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Combatant> SetupTeam(BaseCharacter obj)
        {
            CombatantSlot emptySlot = slots.FirstOrDefault(cs => cs.Empty);
            return emptySlot != null ? await emptySlot.AddCombatant(obj) : null;
        }
    }
}
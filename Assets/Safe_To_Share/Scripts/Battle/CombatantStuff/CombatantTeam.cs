using System.Linq;
using System.Threading.Tasks;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.CombatantStuff
{
    public sealed class CombatantTeam : MonoBehaviour
    {
        [SerializeField] CombatantSlot[] slots;

#if UNITY_EDITOR
        void OnValidate() => slots = GetComponentsInChildren<CombatantSlot>();
#endif

        public void FirstSetup()
        {
            foreach (var slot in slots)
                slot.EmptySlot();
        }

        public async Task<Combatant> SetupTeam(BaseCharacter obj)
        {
            var emptySlot = slots.FirstOrDefault(cs => cs.Empty);
            return emptySlot is not null ? await emptySlot.AddCombatant(obj) : null;
        }
    }
}
using System.Threading.Tasks;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.CombatantStuff {
    public sealed class CombatantSlot : MonoBehaviour {
        [SerializeField] Combatant myCombatant;
        public bool Empty { get; private set; } = true;

        public async Task<Combatant> AddCombatant(BaseCharacter character) {
            Empty = false;
            myCombatant.gameObject.SetActive(true);
            await myCombatant.Setup(character);
            return myCombatant;
        }

        public void EmptySlot() {
            myCombatant.gameObject.SetActive(false);
            Empty = true;
        }
    }
}
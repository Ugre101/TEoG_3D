using Character;
using UnityEngine;

namespace Battle.CombatantStuff
{
    public class CombatantSlot : MonoBehaviour
    {
        [SerializeField] Combatant myCombatant;
        public bool Empty { get; private set; } = true;

        public Combatant AddCombatant(BaseCharacter character)
        {
            Empty = false;
            myCombatant.Setup(character);
            myCombatant.gameObject.SetActive(true);
            return myCombatant;
        }

        public void EmptySlot()
        {
            myCombatant.gameObject.SetActive(false);
            Empty = true;
        }

    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.UI
{
    public class WhoseTurnIcons : MonoBehaviour
    {
        [SerializeField] WhoseTurnIcon icon;

        readonly List<Paired> pairedList = new();

        public void FirstSetup()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        public void AddCombatant(CombatCharacter character)
        {
            WhoseTurnIcon turnIcon = Instantiate(icon, transform);
            turnIcon.Setup(character.Ally, character.Character.Identity.FirstName);
            pairedList.Add(new Paired(turnIcon, character));
            RefreshList();
        }

        public void RefreshList()
        {
            var orderBySpeed = pairedList.OrderByDescending(c => c.Character.SpeedAccumulated).ToArray();
            for (int i = 0; i < orderBySpeed.Length; i++)
                orderBySpeed[i].Icon.transform.SetSiblingIndex(i);
        }

        readonly struct Paired
        {
            public readonly WhoseTurnIcon Icon;
            public readonly CombatCharacter Character;

            public Paired(WhoseTurnIcon icon, CombatCharacter character)
            {
                Icon = icon;
                Character = character;
            }
        }
    }
}
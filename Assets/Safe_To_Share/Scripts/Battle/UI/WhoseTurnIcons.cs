using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.UI
{
    public sealed class WhoseTurnIcons : MonoBehaviour
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
            var turnIcon = Instantiate(icon, transform);
            turnIcon.Setup(character.Ally, character.Character.Identity.FirstName);
            pairedList.Add(new Paired(turnIcon, character));
            RefreshList();
        }

        public void RefreshList()
        {
            pairedList.Sort((paired, paired1) => paired1.Character.SpeedAccumulated.CompareTo(paired.Character.SpeedAccumulated));
            for (var i = 0; i < pairedList.Count; i++)
                pairedList[i].Icon.transform.SetSiblingIndex(i);
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
using System.Linq;
using Character;
using Character.SkillsAndSpells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class AttackButtons : MonoBehaviour
    {
        const int MAXRow = 1, MINRow = 0;
        [SerializeField] AttackBtn[] buttons;
        [SerializeField] Button up, down;
        [SerializeField] TextMeshProUGUI currentRowText;

        AbilityBook currentBook;
        int currentRow;

        int CurrentRow
        {
            get => currentRow;
            set => currentRow = Mathf.Clamp(value, MINRow, MAXRow);
        }

# if UNITY_EDITOR
        void OnValidate() => buttons = GetComponentsInChildren<AttackBtn>();
#endif

        public void FirstSetup()
        {
            foreach (AttackBtn t in buttons)
                t.FirstSetup();

            up.onClick.AddListener(ScrollUp);
            down.onClick.AddListener(ScrollDown);
            UpdateRowButtonsAndText();
        }

        public void Setup(ControlledCharacter playerControlled)
        {
            currentBook = playerControlled.AndSpellBook;
            BindAbilities();
        }

        void BindAbilities()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                AttackBtn btn = buttons[i];
                int bindIndex = CurrentRow > 0 ? buttons.Length * CurrentRow - 1 + i : i;
                btn.SetId(bindIndex);
                string boundAbility = currentBook.BoundAbilities[bindIndex];
                if (!string.IsNullOrEmpty(boundAbility) &&
                    currentBook.Abilities.FirstOrDefault(a => a == boundAbility) is { } ability)
                    btn.BindAbility(ability);
                else
                    btn.Clear();
            }
        }

        void ScrollUp()
        {
            CurrentRow++;
            UpdateRowButtonsAndText();
            BindAbilities();
        }

        void UpdateRowButtonsAndText()
        {
            up.gameObject.SetActive(CurrentRow != MAXRow);
            down.gameObject.SetActive(CurrentRow != MINRow);
            currentRowText.text = CurrentRow.ToString();
        }

        void ScrollDown()
        {
            CurrentRow--;
            UpdateRowButtonsAndText();
            BindAbilities();
        }
    }
}
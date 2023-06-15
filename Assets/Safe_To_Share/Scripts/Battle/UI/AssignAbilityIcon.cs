using System;
using Battle;
using Safe_To_Share.Scripts.Battle.SkillsAndSpells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Battle.UI {
    public sealed class AssignAbilityIcon : MonoBehaviour, IPointerEnterHandler {
        [SerializeField] Image icon;
        [SerializeField] Button btn;
        Ability ability;
        AttackBtn bindTo;

        void Start() => btn.onClick.AddListener(BindAbility);

        public void OnPointerEnter(PointerEventData eventData) {
            if (ability == null) return;
            AbilityLastHovered?.Invoke(ability);
            // Display ability info
        }

        public static event Action<Ability> AbilityLastHovered;
        public static event Action AbilityBound;

        public void Setup(Ability parAbility, AttackBtn parBindTo) {
            gameObject.SetActive(true);
            ability = parAbility;
            bindTo = parBindTo;
            icon.sprite = parAbility.Icon;
        }

        void BindAbility() {
            if (ability == null)
                return;
            bindTo.BindNewAbility(ability);
            AbilityBound?.Invoke();
        }

        public void ChangeButton(AttackBtn obj) => bindTo = obj;
    }
}
using System;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level {
    public abstract class BaseLevelButton : MonoBehaviour, IPointerExitHandler {
        [SerializeField] protected Image rune;
        [SerializeField] protected TextMeshProUGUI btnText;
        [SerializeField] Button btn;
        protected bool Afford = false;
        protected PlayerHolder PlayerHolder;
        protected Player Player => PlayerHolder.Player;
        void Start() => btn.onClick.AddListener(OnClick);


        public void OnPointerExit(PointerEventData eventData) => StopShowPerkInfo?.Invoke();

        public abstract void Setup(PlayerHolder player);

        protected abstract void CanAfford(int obj);

        protected abstract void OnClick();

        public static event Action StopShowPerkInfo;
    }
}
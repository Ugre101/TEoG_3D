using System;
using AvatarStuff.Holders;
using Character.PlayerStuff;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUIAndMenus.Menus.Level
{
    public abstract class BaseLevelButton : MonoBehaviour, IPointerExitHandler
    {
        [SerializeField] protected Image rune;
        [SerializeField] protected TextMeshProUGUI btnText;
        [SerializeField] Button btn;
        protected bool Afford = false;
        protected PlayerHolder player;

        void Start()
        {
            btn.onClick.AddListener(OnClick);
        }

        public abstract void Setup(PlayerHolder player);


        public void OnPointerExit(PointerEventData eventData) => StopShowPerkInfo?.Invoke();

        protected abstract void CanAfford(int obj);

        protected abstract void OnClick();

        public static event Action StopShowPerkInfo;
    }
}
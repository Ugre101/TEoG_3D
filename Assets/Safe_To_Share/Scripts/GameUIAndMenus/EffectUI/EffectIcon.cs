using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.EffectUI
{
    public abstract class EffectIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image icon;
        protected abstract string HoverText { get; }
        public void OnPointerEnter(PointerEventData eventData) => HoverInfo?.Invoke(HoverText, transform.position.x);

        public void OnPointerExit(PointerEventData eventData) => StopHoverInfo?.Invoke();
        public event Action<string, float> HoverInfo;
        public event Action StopHoverInfo;
    }
}
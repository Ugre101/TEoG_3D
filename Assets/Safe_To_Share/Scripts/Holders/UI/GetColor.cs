using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Holders.UI {
    public sealed class GetColor : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        [SerializeField] GameObject pointer;
        [SerializeField] Texture2D texture2D;
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Image choosenColor;
        Vector2 startPos;

        public void OnBeginDrag(PointerEventData eventData) => startPos = pointer.transform.position;

        public void OnDrag(PointerEventData eventData) {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null,
                    out var point)) return;
            pointer.transform.position = eventData.position;
            var rect = rectTransform.rect;
            var width = rect.width;
            var height = rect.height;
            point += new Vector2(width / 2, height / 2);

            var chosen = texture2D.GetPixel((int)point.x, (int)point.y);
            choosenColor.color = chosen;
            NewColor?.Invoke(chosen);
        }

        public void OnEndDrag(PointerEventData eventData) => pointer.transform.position = startPos;
        public event Action<Color> NewColor;
    }
}
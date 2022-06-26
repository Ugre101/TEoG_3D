using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetColor : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject pointer;
    [SerializeField] Texture2D texture2D;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image choosenColor;
    Vector2 startPos;
    public event Action<Color> NewColor;

    public void OnBeginDrag(PointerEventData eventData) => startPos = pointer.transform.position;

    public void OnDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null,
            out Vector2 point)) return;
        pointer.transform.position = eventData.position;
        var rect = rectTransform.rect;
        float width = rect.width;
        float height = rect.height;
        point += new Vector2(width / 2, height / 2);

        Color chosen = texture2D.GetPixel((int)point.x, (int)point.y);
        choosenColor.color = chosen;
        NewColor?.Invoke(chosen);
    }

    public void OnEndDrag(PointerEventData eventData) => pointer.transform.position = startPos;
}
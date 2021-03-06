using Items;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.Menus.Inventory
{
    public abstract class ItemBaseHoverText : MonoBehaviour
    {
        [SerializeField] RectTransform rect;
        [SerializeField] TextMeshProUGUI title, desc, value;
        [SerializeField] Vector2 offset;

        protected void ShowItem(Item item, Vector2 vector2)
        {
            if (item == null)
                return;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, vector2, null, out Vector3 point))
            {
                var transRect = rect.rect;
                point.x += offset.x * (Screen.width / transRect.width);
                point.y += offset.y * (Screen.height / transRect.height);
                transform.position = point;
            }

            gameObject.SetActive(true);
            title.text = item.Title;
            desc.text = item.Desc;
            value.text = $"{item.Value}g";
        }

        protected void StopShowing() => gameObject.SetActive(false);
    }
}
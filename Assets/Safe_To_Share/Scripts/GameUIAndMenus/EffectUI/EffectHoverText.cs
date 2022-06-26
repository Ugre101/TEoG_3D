using TMPro;
using UnityEngine;

namespace GameUIAndMenus.EffectUI
{
    public class EffectHoverText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI hoverText;
        [SerializeField] RectTransform rectTransform;

        public void ShowHoverText(string text, float xPos)
        {
            hoverText.transform.position =
                new Vector3(xPos + rectTransform.rect.width / 2f, hoverText.transform.position.y);
            hoverText.gameObject.SetActive(true);
            hoverText.text = text;
        }

        public void StopHoverText() => hoverText.gameObject.SetActive(false);
    }
}
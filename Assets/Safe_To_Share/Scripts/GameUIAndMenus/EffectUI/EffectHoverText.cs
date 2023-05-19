﻿using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.EffectUI
{
    public sealed class EffectHoverText : MonoBehaviour
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
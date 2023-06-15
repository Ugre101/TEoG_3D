﻿using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level {
    public class BaseLevelMenu : GameMenu {
        [SerializeField] protected TextMeshProUGUI perkPointsLeft;
        [SerializeField] protected Transform content;
        [SerializeField, Range(0.01f, 0.5f),] float zoomRate;
        [SerializeField, Range(0.1f, 1f),] float minZoom;
        [SerializeField] BaseLevelButton[] btns;
        bool firstStart = true;

        void Start() {
            firstStart = false;
            SetupButtons();
        }

        protected virtual void OnEnable() {
            content.localScale = Vector3.one;
            content.localPosition = Vector3.zero;
            if (!firstStart)
                SetupButtons();
        }

#if UNITY_EDITOR
        void OnValidate() => btns = GetComponentsInChildren<BaseLevelButton>();
#endif

        public void OnZoom(InputValue ctx) {
            var newZoom = Mathf.Clamp(content.localScale.x + ctx.Get<float>() * Time.unscaledDeltaTime * zoomRate,
                minZoom, 2f);
            content.localScale = new Vector3(newZoom, newZoom, newZoom);
        }

        void SetupButtons() {
            foreach (var btn in btns)
                btn.Setup(holder);
        }
    }
}
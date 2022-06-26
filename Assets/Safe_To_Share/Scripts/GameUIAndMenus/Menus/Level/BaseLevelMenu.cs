using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUIAndMenus.Menus.Level
{
    public class BaseLevelMenu : GameMenu
    {
        [SerializeField] protected TextMeshProUGUI perkPointsLeft;
        [SerializeField] protected Transform content;
        [SerializeField, Range(0.01f, 0.5f),] float zoomRate;
        [SerializeField, Range(0.1f, 1f),] float minZoom;
        [SerializeField]  private BaseLevelButton[] btns;
        bool firstStart = true;

        protected virtual void OnEnable()
        {
            content.localScale = Vector3.one;
            content.localPosition = Vector3.zero;
            if (!firstStart)
                SetupButtons();
        }

        public void OnZoom(InputValue ctx)
        {
            float newZoom = Mathf.Clamp(content.localScale.x + ctx.Get<float>() * Time.unscaledDeltaTime * zoomRate,
                minZoom, 2f);
            content.localScale = new Vector3(newZoom, newZoom, newZoom);
        }
        void Start()
        {
            firstStart = false;
            SetupButtons();
        }

#if UNITY_EDITOR
        void OnValidate() => btns = GetComponentsInChildren<BaseLevelButton>();
#endif
        
        private void SetupButtons()
        {
            foreach (var btn in btns)
                btn.Setup(holder);
        }
    }
}
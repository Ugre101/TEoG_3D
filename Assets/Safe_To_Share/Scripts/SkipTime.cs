using CustomClasses;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts
{
    public class SkipTime : MonoBehaviour,ICancelMeBeforeOpenPauseMenu
    {
        static int waitTime = 1;
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI waitText;
        [SerializeField] InputActionMap binds = new();
        [SerializeField] Button waitBtn, closeBtn;

        void Start()
        {
            slider.value = waitTime;
            slider.onValueChanged.AddListener(ChangeWait);
            closeBtn.onClick.AddListener(Close);
            waitBtn.onClick.AddListener(Wait);
            UpdateWaitText();
            binds.actions[0].performed += IncreaseTime;
            binds.actions[1].performed += DecreaseTime;
            binds.actions[2].performed += CallWait;
        }
        void IncreaseTime(InputAction.CallbackContext obj) => slider.value++;
        void DecreaseTime(InputAction.CallbackContext obj) => slider.value--;
        void CallWait(InputAction.CallbackContext obj) => Wait();
        public void ToggleSetActive(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                gameObject.SetActive(!gameObject.activeSelf);
        }
        void OnEnable() => binds.Enable();

        void OnDisable() => binds.Disable();

        void OnDestroy() => binds.Dispose();

        void UpdateWaitText() => waitText.text = $"{waitTime}h";

        void Wait()
        {
            DateSystem.PassHour(Mathf.Max(1, waitTime));
            Close();
        }

        void Close() => gameObject.SetActive(false);

        void ChangeWait(float arg0)
        {
            waitTime = Mathf.RoundToInt(arg0);
            UpdateWaitText();
        }

        public bool BlockIfActive()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }
    }
}
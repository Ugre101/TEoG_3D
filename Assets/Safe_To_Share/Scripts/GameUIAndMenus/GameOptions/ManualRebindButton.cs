using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.GameUIAndMenus.GameOptions
{
    public sealed class ManualRebindButton : MonoBehaviour
    {
        [SerializeField] InputActionReference actionRef;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] List<KeyBindButton> keyBindButtons = new();

        void Start()
        {
            if (actionRef == null)
                return;
            for (int i = 0; i < keyBindButtons.Count; i++) keyBindButtons[i].Setup(actionRef, i);
        }

# if UNITY_EDITOR
        void OnValidate()
        {
            if (actionRef == null)
                return;
            title.text = actionRef.action.name;
            //  UpdateBindingDisplay(0);
            //   UpdateBindingDisplay(1);
        }
#endif
    }
}
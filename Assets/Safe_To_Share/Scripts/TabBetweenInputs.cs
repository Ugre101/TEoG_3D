using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts
{
    public sealed class TabBetweenInputs : MonoBehaviour
    {
        [SerializeField] InputAction inputAction;

        EventSystem system;

        void Start()
        {
            system = EventSystem.current;
            inputAction.performed += ctx => Next();
        }

        void OnEnable() => inputAction.Enable();

        void OnDisable() => inputAction.Disable();

        void Next()
        {
            Selectable selected = system.currentSelectedGameObject != null
                ? system.currentSelectedGameObject.GetComponent<Selectable>()
                : null;
            Selectable[] selectables = GetComponentsInChildren<Selectable>();
            if (selectables.Length == 0)
                return;
            int nextIndex = selected == null
                ? 0
                :
                Array.IndexOf(selectables, selected) < selectables.Length - 1
                    ?
                    Array.IndexOf(selectables, selected) + 1
                    :
                    0;

            GameObject nextObject = selectables[nextIndex].gameObject;
            if (nextObject.TryGetComponent(out TMP_InputField inputField))
                inputField.OnPointerClick(new PointerEventData(system));
            system.SetSelectedGameObject(nextObject, new BaseEventData(system));
        }
    }
}
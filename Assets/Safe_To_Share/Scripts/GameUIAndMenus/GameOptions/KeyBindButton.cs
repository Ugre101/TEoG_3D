using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameUIAndMenus.GameOptions
{
    public class KeyBindButton : MonoBehaviour
    {
        [SerializeField] Button btn;
        [SerializeField] TextMeshProUGUI btnTitle;
        int index;
        InputActionRebindingExtensions.RebindingOperation rebindingOperation;
        InputActionReference reference;

        public Button Btn => btn;

        void OnDisable()
        {
            rebindingOperation?.Cancel();
            CleanUp();
        }

        internal void Setup(InputActionReference reference, int i)
        {
            this.reference = reference;
            index = i;
            UpdateText();
            btn.onClick.AddListener(StartRebind);
        }

        void UpdateText() =>
            btnTitle.text = reference.action.bindings.Count > index
                ? InputControlPath.ToHumanReadableString(reference.action.bindings[index].effectivePath,
                    InputControlPath.HumanReadableStringOptions.UseShortNames)
                : string.Empty;

        void StartRebind()
        {
            rebindingOperation?.Dispose();
            InputAction action = reference.action;
            if (action.bindings.Count + 1 <= index)
            {
                btnTitle.text = "Wrong button";
                return;
            }

            print($"{action.bindings.Count} <= {index}");
            if (action.bindings.Count <= index)
            {
                print("Had no bindings");
                action.AddBinding("<Keyboard>p");
            }

            action.Disable();
            btnTitle.text = "Press Key";
            rebindingOperation = action.PerformInteractiveRebinding(index).WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(operation =>
                {
                    print("Canceled");
                    CleanUp();
                    RebindCancelled();
                    UpdateText();
                })
                .OnComplete(operation =>
                {
                    print($"Completed rebound to {operation.selectedControl}");
                    if (CheckConflict())
                        RebindCancelled();
                    else
                        RebindCompleted();

                    UpdateText();
                    CleanUp();
                });
            rebindingOperation.Start();
        }

        void RebindCompleted()
        {
            rebindingOperation.Dispose();
            reference.action.Enable();
        }

        bool CheckConflict()
        {
            int count = reference.action.actionMap.actions.Count(a =>
                a.bindings.Any(ia => ia.path == reference.action.bindings[index].path));
            return count > 1;
        }

        void RebindCancelled()
        {
            reference.action.RemoveBindingOverride(index);
            if (index > 0)
                reference.action.ChangeBinding(index).Erase();
            reference.action.Enable();
        }

        void CleanUp()
        {
            rebindingOperation?.Dispose();
            rebindingOperation = null;
        }
    }
}
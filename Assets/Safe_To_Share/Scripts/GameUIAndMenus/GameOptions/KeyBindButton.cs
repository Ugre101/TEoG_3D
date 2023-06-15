using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.GameOptions {
    public sealed class KeyBindButton : MonoBehaviour {
        [SerializeField] Button btn;
        [SerializeField] TextMeshProUGUI btnTitle;
        int index;
        InputActionRebindingExtensions.RebindingOperation rebindingOperation;
        InputActionReference reference;

        public Button Btn => btn;

        void OnDisable() {
            rebindingOperation?.Cancel();
            CleanUp();
        }

        internal void Setup(InputActionReference reference, int i) {
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

        void StartRebind() {
            rebindingOperation?.Dispose();
            var action = reference.action;
            if (action.bindings.Count + 1 <= index) {
                btnTitle.text = "Wrong button";
                return;
            }

            if (action.bindings.Count <= index)
                action.AddBinding("<Keyboard>p");

            action.Disable();
            btnTitle.text = "Press Key";
            rebindingOperation = action.PerformInteractiveRebinding(index).WithCancelingThrough("<Keyboard>/escape")
                                       .OnMatchWaitForAnother(0.1f)
                                       .OnCancel(operation => {
                                            CleanUp();
                                            RebindCancelled();
                                            UpdateText();
                                        })
                                       .OnComplete(operation => {
                                            if (CheckConflict(operation.action.bindings[index].effectivePath))
                                                RebindCancelled();
                                            else
                                                RebindCompleted();

                                            UpdateText();
                                            CleanUp();
                                        });
            rebindingOperation.Start();
        }

        void RebindCompleted() {
            rebindingOperation.Dispose();
            reference.action.Enable();
        }

        bool CheckConflict(string path) {
            foreach (var inputAction in reference.action.actionMap) {
                if (inputAction == reference.action)
                    continue;
                foreach (var binding in inputAction.bindings) {
                    if (binding.hasOverrides && binding.overridePath == path)
                        return true;
                    if (binding.path == path)
                        return true;
                }
            }

            return false;
        }

        void RebindCancelled() {
            reference.action.RemoveBindingOverride(index);
            if (index > 0)
                reference.action.ChangeBinding(index).Erase();
            reference.action.Enable();
        }

        void CleanUp() {
            rebindingOperation?.Dispose();
            rebindingOperation = null;
        }
    }
}
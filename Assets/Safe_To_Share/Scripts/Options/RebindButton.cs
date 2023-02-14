using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Options
{
    public class RebindButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, bind1Text, bind2Text;
        [SerializeField] Button bind1, bind2;
        InputAction action;
        InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        void OnDisable()
        {
            rebindingOperation?.Cancel();
            CleanUp();
        }

        public void Setup(InputAction inputAction)
        {
            action = inputAction;
            title.text = inputAction.name;
            bind1.onClick.AddListener(() => StartRebind(0));
            UpdateBindingDisplay(0);
            bind2.onClick.AddListener(() => StartRebind(1));
            UpdateBindingDisplay(1);
        }

        void StartRebind(int i)
        {
            rebindingOperation?.Dispose();
            action.Disable();
            switch (i)
            {
                case 0:
                    bind1Text.text = string.Empty;
                    break;
                case 1:
                {
                    bind2Text.text = string.Empty;
                    if (action.bindings.Count < 2)
                        action.AddBinding("temp");
                    break;
                }
            }

            rebindingOperation = action.PerformInteractiveRebinding(i).WithCancelingThrough("<Keyboard>/escape")
                .WithControlsExcluding("<Mouse>").OnMatchWaitForAnother(0.1f)
                .OnCancel(_ =>
                {
                    CleanUp();
                    RebindCancelled(i);
                    UpdateBindingDisplay(i);
                })
                .OnComplete(operation =>
                {
                    if (CheckConflict(operation.action.bindings[i].effectivePath))
                        RebindCancelled(i);
                    else
                        RebindCompleted();

                    UpdateBindingDisplay(i);
                    CleanUp();
                });
            rebindingOperation.Start();
        }

   
        void RebindCancelled(int i)
        {
            action.RemoveBindingOverride(i);
            if (i > 0)
                action.ChangeBinding(i).Erase();
            action.Enable();
        }

        bool CheckConflict(string path)
        {
            foreach (var inputAction in action.actionMap)
            {
                if (inputAction == action)
                    continue;
                foreach (var binding in inputAction.bindings)
                {
                    if (binding.hasOverrides && binding.overridePath == path)
                        return true;
                    if (binding.path == path)
                        return true;
                }
            }

            return false;
            // InputAction other = action.actionMap.actions.FirstOrDefault(a =>
            //     a.bindings.Any(ia => ia.path == action.bindings[i].path) && a != action);
        }

        void CleanUp()
        {
            rebindingOperation?.Dispose();
            rebindingOperation = null;
        }

        void UpdateBindingDisplay(int i)
        {
            switch (i)
            {
                case 0:
                    bind1Text.text = action.bindings.Count > 0
                        ? InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath,
                            InputControlPath.HumanReadableStringOptions.OmitDevice)
                        : string.Empty;
                    break;
                case 1:
                    bind2Text.text = action.bindings.Count > i
                        ? InputControlPath.ToHumanReadableString(action.bindings[i].effectivePath,
                            InputControlPath.HumanReadableStringOptions.OmitDevice)
                        : string.Empty;
                    break;
            }
        }

        void RebindCompleted()
        {
            rebindingOperation.Dispose();
            action.Enable();
        }
    }
}
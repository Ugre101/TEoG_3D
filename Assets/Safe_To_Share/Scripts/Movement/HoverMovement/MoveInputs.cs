using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class MoveInputs : MonoBehaviour
    {
        public Vector2 Move { get; private set; }

        public bool Sprinting { get; private set; }

        public bool Crunching { get; private set; }

        public bool Jumping { get; private set; }
        public bool Moving => Move.magnitude > 0;

        bool autoRunning;
        public void OnAutoRun(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            Move = autoRunning ? Vector2.zero : Vector2.up;
            autoRunning = !autoRunning;

        }
        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Move = ctx.ReadValue<Vector2>().normalized;
                if (autoRunning)
                    autoRunning = false;
            }
            else if (ctx.canceled)
                Move = Vector2.zero;
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                Sprinting = true;
            else if (ctx.canceled)
                Sprinting = false;
        }

        public void OnCrunching(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                Crunching = true;
            else if (ctx.canceled)
                Crunching = false;
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                Jumping = true;
            else if (ctx.canceled)
                Jumping = false;
        }
    }
}
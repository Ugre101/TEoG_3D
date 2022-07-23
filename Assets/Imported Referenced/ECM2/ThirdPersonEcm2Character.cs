using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Movement.ECM2.Source.Characters
{
    /// <summary>
    ///     ThirdPersonCharacter.
    ///     This extends the Character class to add controls for a typical third person movement.
    /// </summary>
    public class ThirdPersonEcm2Character : ECM2Character
    {
        #region FIELDS

        [SerializeField] Transform cameraTargetTransform;
        #endregion

        #region METHODS

        protected override void UpdateRotation()
        {
            if (firstPerson)
                characterMovement.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            else if (autoRun || allowCameraSteer)
            {
                 SetRotation(Quaternion.Euler(0, cameraTargetTransform.eulerAngles.y, 0));
                    cameraTargetTransform.localEulerAngles = new Vector3(cameraTargetTransform.localEulerAngles.x, 0, 0);
            }
            else
                base.UpdateRotation();
        }

        #endregion

        #region PROPERTIES

        #endregion


        #region INPUT ACTION HANDLERS

        public void SetFirstPersonMode(bool value) => firstPerson = value;

        bool allowCameraSteer;
        bool firstPerson;

        /// <summary>
        ///     Gets the controller look input value.
        ///     Return its current value or zero if no valid InputAction found.
        /// </summary>
        public override void SetInputVec(InputAction.CallbackContext ctx)
        {
            allowCameraSteer = false;
            //base.SetInputVec(ctx);
            if (ctx.performed)
                Move(ctx);
            else if (ctx.canceled)
                Canceled();
        }

        void Canceled()
        {
            switch (autoRun)
            {
                case false:
                    Stop();
                    break;
                case true:
                    inputVec = Vector2.up;
                    break;
            }
        }

        void Move(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            if (autoRun && input.y != 0)
                autoRun = false;
            if (!firstPerson && ThirdPersonControls(input))
                return;
            if (ThirdPersonMovementSettings.Strafe && input.y != 0 && input.x != 0)
                allowCameraSteer = true;
            inputVec = input;
        }

        bool ThirdPersonControls(Vector2 input)
        {
            if (input.y != 0)
            {
                if (input.x == 0)
                    allowCameraSteer = true;
                return false;
            }

            if (autoRun)
            {
                inputVec = input + Vector2.up;
                return true;
            }

            yaw = input.x * 1.4f;
            return true;
        }

        public void Stop()
        {
            yaw = 0;
            inputVec = Vector2.zero;
        }

        bool autoRun;

        public void ToggleAutoRun(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            if (!autoRun || inputVec.y == 0)
            {
                autoRun = true;
                inputVec.y = 1f;
                return;
            }

            switch (autoRun)
            {
                case true when inputVec.y > 0:
                    autoRun = false;
                    inputVec.y = 0f;
                    break;
                case true:
                    inputVec.y = 1f;
                    break;
            }
        }

        public void MouseJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && !(EventSystem.current && EventSystem.current.IsPointerOverGameObject()))
                Jump();
        }

        #endregion
    }
}
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField, Range(1f, 3f),] float minDistance;
        [SerializeField, Range(5f, 15f),] float maxDistance;
        [SerializeField, Range(5f, 15f),] float zoomRate;

        [Space, SerializeField, Range(25f, 45f),]
        float spinRate;

        [SerializeField, Range(1f, 3f),] float spinBoost = 2f;

        [Space, SerializeField,] float minTilt, maxTilt = 60f;
        [SerializeField, Range(0.01f, 0.5f),] float tiltRate = 0.5f;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        Cinemachine3rdPersonFollow followCam;
        bool isBoostingSpin;
        bool lookUnLocked;
        float? rotate;
        float? tilt;
        float? zoom;

        void Start() => followCam = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        void Update()
        {
            if (zoom.HasValue)
                SetDistance(zoom.Value * Time.deltaTime);
            if (rotate.HasValue)
                RotateTarget(rotate.Value * spinRate * Time.deltaTime);
            if (tilt.HasValue)
                TiltTarget(tilt.Value);
        }

        public void OnRotateAndZoom(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Vector2 movement = ctx.ReadValue<Vector2>();
                zoom = movement.y * zoomRate;
                rotate = -movement.x;
            }
            else if (ctx.canceled)
            {
                zoom = null;
                rotate = null;
            }
        }

        public void OnTilt(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                tilt = ctx.ReadValue<float>();
            else if (ctx.canceled)
                tilt = null;
        }

        public void LockMouse(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Cursor.lockState = CursorLockMode.Locked;
                lookUnLocked = true;
            }
            else if (ctx.canceled)
            {
                Cursor.lockState = CursorLockMode.None;
                lookUnLocked = false;
            }
        }

        public void Look(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || !lookUnLocked)
                return;
            Vector2 move = ctx.ReadValue<Vector2>();
            RotateTarget(move.x * spinRate * Time.deltaTime);
            TiltTarget(move.y);
        }

        void SetDistance(float newDistance) => followCam.CameraDistance =
            Mathf.Clamp(followCam.CameraDistance - newDistance, minDistance, maxDistance);

        public void BoostSpinRate(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                isBoostingSpin = true;
            else if (ctx.canceled)
                isBoostingSpin = false;
        }

        void RotateTarget(float value) => target.Rotate(0, isBoostingSpin ? value * spinBoost : value, 0, Space.World);

        void TiltTarget(float value)
        {
            Vector3 oldRot = target.eulerAngles;
            oldRot.x = Mathf.Clamp(oldRot.x + value * tiltRate, minTilt, maxTilt);
            target.eulerAngles = oldRot;
        }
    }
}
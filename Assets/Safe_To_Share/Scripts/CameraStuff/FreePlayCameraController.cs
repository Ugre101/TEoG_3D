using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Movement.ECM2.Source.Characters;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.CameraStuff
{
    public class FreePlayCameraController : MonoBehaviour
    {
        [SerializeField] ThirdPersonEcm2Character mover;
        [SerializeField] FirstPersonCamera firstPerson;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        Cinemachine3rdPersonFollow followCam;

        [Header("Settings")]
        [SerializeField,Range(float.Epsilon,3f)] float minDistance = 3;
        [SerializeField] float maxDistance = 20;
        [SerializeField] private Transform target;
        [SerializeField, Range(float.Epsilon, 0.05f)] float tiltRate = 0.02f;
        [SerializeField, Range(float.Epsilon, 0.1f)] float rorateRate = 0.04f;
        [SerializeField, Range(float.Epsilon, 0.05f)] float zoomRate = 0.05f;
        [SerializeField, Range(float.Epsilon, 0.05f)] float elevationRate = 0.01f;

        Vector2 mouseLookInput;
        bool isCursorLocked;
        static float Sensitivity => ThirdPersonCameraSettings.Sensitivity;
        static bool InvertVerticalAxis => ThirdPersonCameraSettings.InvertVerticalAxis;

        private void Start() => followCam = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        Coroutine elevationRoutine;
        readonly WaitForSecondsRealtime waitForSecondsRealtime = new(0.01f);
        IEnumerator TargetElevation(float change)
        {
            bool loop = true;
            while (loop)
            {
                float y = Mathf.Clamp(target.localPosition.y + change, 0.1f, 3f);
                if (Math.Abs(y - 0.1f) < 0.01f || Math.Abs(y - 3f) < 0.01f) loop = false;
                target.localPosition = new Vector3(0, y, 0);
                yield return waitForSecondsRealtime;
            }
        }
        private void OnEnable()
        {
            mover.SetRotationMode(RotationMode.OrientToMovement);
            mover.SetFirstPersonMode(false);
        }
        public void OnRaiseElevation(InputAction.CallbackContext ctx)
        {
            if (GameManager.Paused) return;
            if (ctx.performed)
                elevationRoutine = StartCoroutine(TargetElevation(elevationRate));
            else if (ctx.canceled)
                StopCoroutine(elevationRoutine);
        }
        public void OnLowerCameraHeight(InputAction.CallbackContext ctx)
        {
            if (GameManager.Paused) return;
            if (ctx.performed)
                elevationRoutine = StartCoroutine(TargetElevation(-elevationRate));
            else if (ctx.canceled)
                StopCoroutine(elevationRoutine);
        }

        public void OnCursorLock(InputAction.CallbackContext context)
        {
            if (GameManager.Paused) return;
            if (!IsPointerOverUIObject() && context.performed)
                LockCursor();
            else if (context.canceled)
                UnlockCursor();
        }
        private static bool IsPointerOverUIObject()
        {
            if (EventSystem.current == null)
                return false;
            PointerEventData eventDataCurrentPosition = new(EventSystem.current)
            {
                position = Pointer.current.position.ReadValue(),
            };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        public void OnLook(InputAction.CallbackContext ctx)
        {
            if (GameManager.Paused) return;
            if (!ctx.performed)
                return;
            if (ThirdPersonCameraSettings.AlwaysLook && mover.GetVelocity().magnitude > 0)
                LockCursor();
            else if (!IsCursorLocked())
                return;
            mouseLookInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
            if (mouseLookInput.y != 0)
                TiltTarget(mouseLookInput.y);
            if (mouseLookInput.x != 0)
                RotateTarget(InvertVerticalAxis ? -mouseLookInput.x :  mouseLookInput.x);
        }

        public void OnMouseScroll(InputAction.CallbackContext ctx)
        {
            if (GameManager.Paused) return;
            if (ctx.performed)
                SetDistance(ctx.ReadValue<float>() * zoomRate);
        }

        private void SetDistance(float newDistance)
        {
            followCam.CameraDistance = Mathf.Clamp(followCam.CameraDistance - newDistance, minDistance, maxDistance);
            if (Math.Abs(followCam.CameraDistance - minDistance) < float.Epsilon)
            {
                gameObject.SetActive(false);
                firstPerson.gameObject.SetActive(true);
            }
        }

        public virtual void OnCursorUnlock(InputAction.CallbackContext context)
        {
            if (context.performed)
                UnlockCursor();
        }

        public void LockCursor()
        {
            isCursorLocked = true;
            UpdateCursorLockState();
        }

        public void UnlockCursor()
        {
            isCursorLocked = false;
            //  UnLockCameraBehindTarget();
            UpdateCursorLockState();
        }

        protected virtual void UpdateCursorLockState()
        {
            Cursor.visible = !isCursorLocked;
            Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        bool IsCursorLocked() => isCursorLocked;

        private void RotateTarget(float value)
        {
            target.rotation *= Quaternion.AngleAxis(value * rorateRate * Sensitivity, Vector3.up);
            var targetTrans = target.transform;
            var angles = targetTrans.localEulerAngles;
            angles.z = 0;
            targetTrans.localEulerAngles = angles;
        }

        private void TiltTarget(float value)
        {
            target.rotation *= Quaternion.AngleAxis(value * tiltRate * Sensitivity, Vector3.right);
            var angles = target.transform.localEulerAngles;
            angles.z = 0;
            var x = angles.x;
            if (x is > 180 and < 320)
                angles.x = 320;
            else if (x is < 180 and > 55)
                angles.x = 55;

            target.transform.localEulerAngles = angles;
        }
    }
}
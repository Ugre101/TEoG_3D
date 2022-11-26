using System;
using System.Collections.Generic;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AvatarStuff.Holders
{
    public class PlayerFreeRoamInteraction : MonoBehaviour
    {
        const int FrameLimit = 10;
        [SerializeField] Button optionButton;
        [SerializeField] TextMeshProUGUI btnText;
        [SerializeField] PlayerHolder playerHolder;
        [SerializeField] Camera cam;

        [Header("Settings"), SerializeField,]
        
        List<Vector3> rays = new();

        [SerializeField, Range(0.1f, 3f),] float rayDist = 1f;
        [SerializeField] LayerMask searchLayers;
        [SerializeField] InputActionReference hotKey;
        [SerializeField] float cameraRayDist = 12f;
        NativeArray<RaycastCommand> commands;
        JobHandle handle;
        bool hasHit;
        IInteractable lastHit;
        Vector3 lastHitPos;
        NativeArray<RaycastHit> results;
        float stopHoverDist;

        bool timeToCheck;

        void Start()
        {
            results = new NativeArray<RaycastHit>(rays.Count + 1, Allocator.Persistent);
            commands = new NativeArray<RaycastCommand>(rays.Count + 1, Allocator.Persistent);

            optionButton.onClick.AddListener(DoInteraction);
            stopHoverDist = rayDist * 3f;
        }

      
        void Update()
        {
            if (Time.frameCount % FrameLimit != 0)
                return;
            if (timeToCheck)
            {
                if (CheckRayCastResult())
                    ShowOptionsShowFor(lastHit);
                timeToCheck = false;
            }
            else
            {
                ScheduleRaycast();
            }

            if (hasHit && Vector3.Distance(lastHitPos, transform.position) > stopHoverDist)
                StopShowText();
        }

        void OnDestroy()
        {
            results.Dispose();
            commands.Dispose();
        }

        void DoInteraction() => lastHit?.DoInteraction(playerHolder.Player);

        void StopShowText()
        {
            optionButton.gameObject.SetActive(false);
            ClearLastHit();
            hasHit = false;
        }

    

        void ShowOptionsShowFor(IInteractable interactable)
        {
            btnText.text = $"{interactable.HoverText(playerHolder.Player)} [{HotkeyHumanReadableString()}]";
            optionButton.gameObject.SetActive(true);
            hasHit = true;
        }

        string HotkeyHumanReadableString() => InputControlPath.ToHumanReadableString(hotKey.action.bindings[0].path,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        Ray? lastCamRay;
        Vector3? lastPos;
        Vector3? lastDir;
        void ScheduleRaycast()
        {
            Vector3 transformDirection = transform.TransformDirection(Vector3.forward);
            Ray camRay = cam.ScreenPointToRay(Pointer.current.position.ReadValue());

            if (ShouldISkipBodyRay(transformDirection))
            {
                for (int i = 0; i < rays.Count; i++)
                    commands[i] = new RaycastCommand(transform.position + rays[i], transformDirection, rayDist,
                        searchLayers);
                lastPos = transform.position;
                lastDir = transformDirection;
            }

            if (ShouldISkipPointerRay(camRay))
            {
                commands[rays.Count] = new RaycastCommand(camRay.origin, camRay.direction, cameraRayDist, searchLayers);
                lastCamRay = camRay;
            }

            handle = RaycastCommand.ScheduleBatch(commands, results, 1);
            timeToCheck = true;
        }

        bool ShouldISkipPointerRay(Ray camRay) =>
            !lastCamRay.HasValue || lastCamRay.Value.origin != camRay.origin ||
            lastCamRay.Value.direction != camRay.direction;

        bool ShouldISkipBodyRay(Vector3 transformDirection) =>
            !lastPos.HasValue || !lastDir.HasValue || lastPos.Value != transform.position ||
            lastDir.Value != transformDirection;

        bool CheckRayCastResult()
        {
            handle.Complete();
            // Copy the result. If batchedHit.collider is null there was no hit
            foreach (RaycastHit hit in results)
            {
                if (hit.collider == null) continue;
                if (!hit.transform.TryGetComponent(out IInteractable interactable)) continue;
                lastHitPos = hit.transform.position;
                SetLastHit(interactable);
                return true;
            }

            return false;
        }

        void SetLastHit(IInteractable interactable)
        {
            lastHit = interactable;
            lastHit.UpdateHoverText += ShowOptionsShowFor;
            lastHit.RemoveIInteractableHit += StopShowText;
        }
        void ClearLastHit()
        {
            if (lastHit == null) return; 
            lastHit.UpdateHoverText -= ShowOptionsShowFor;
            lastHit.RemoveIInteractableHit -= ClearLastHit;
            lastHit = null;
        }
        public void OnInterAction(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                lastHit?.DoInteraction(playerHolder.Player);
        }
    }
}
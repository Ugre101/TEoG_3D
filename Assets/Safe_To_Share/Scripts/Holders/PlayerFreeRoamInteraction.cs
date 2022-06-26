using System.Collections.Generic;
using System.Linq;
using Character.PlayerStuff;
using TMPro;
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
        [Header("Settings")]
        [SerializeField] List<Vector3> rays = new();
        [SerializeField, Range(0.1f, 3f),] float rayDist = 1f;
        [SerializeField] LayerMask searchLayers;
        [SerializeField] InputActionReference hotKey;
        Vector3 lastHitPos;
        IInteractable lastHit;
        bool hasHit;
        float stopHoverDist;
        void Start()
        {
            optionButton.onClick.AddListener(DoInteraction);
            stopHoverDist = rayDist * 3f;
        }

        private void DoInteraction() => lastHit?.DoInteraction(playerHolder.Player);

        void Update()
        {
            if (Time.frameCount % FrameLimit != 0)
                return;
            if (CastRayHits())
            {
                ShowOptionsShowFor(lastHit);
                return;
            }
            if (hasHit && Vector3.Distance(lastHitPos, transform.position) > stopHoverDist)
                StopShowText();
        }

        void StopShowText()
        {
            optionButton.gameObject.SetActive(false);
            lastHit = null;
            hasHit = false;
        }

        void ShowOptionsShowFor(IInteractable interactable)
        {
            btnText.text = $"{interactable.HoverText(playerHolder.Player)} [{HotkeyHumanReadableString()}]";
            optionButton.gameObject.SetActive(true);
            hasHit = true;
        }

        string HotkeyHumanReadableString() => InputControlPath.ToHumanReadableString(hotKey.action.bindings[0].path, InputControlPath.HumanReadableStringOptions.OmitDevice);

        bool CastRayHits() => rays.Any(CastBodyRay) || CastCameraRay();

        private bool CastCameraRay()
        {
            Ray camRay = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
            if (!CameraLooksAtInteractable(camRay, out RaycastHit camHit, out IInteractable camInteractable))
                return false;
            lastHitPos = camHit.point;
            lastHit = camInteractable;
            return true;
        }

        private bool CameraLooksAtInteractable(Ray camRay, out RaycastHit camHit, out IInteractable camInteractable)
        {
            camInteractable = null;
            return Physics.Raycast(camRay, out camHit, 100f, searchLayers) &&
                   camHit.transform.TryGetComponent(out camInteractable) &&
                   Vector3.Distance(camHit.point, transform.position) <= rayDist * 3f;
        }

        bool CastBodyRay(Vector3 offset)
        {
            Ray ray = new Ray(transform.position + offset, transform.TransformDirection(Vector3.forward));
            if (!Physics.Raycast(ray, out RaycastHit hit, rayDist, searchLayers) || !hit.transform.TryGetComponent(out IInteractable interactable))
                return false;
            lastHitPos = hit.transform.position;
            lastHit = interactable;
            return true;
        }

        

        public void OnInterAction(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                lastHit?.DoInteraction(playerHolder.Player);
        }
    }
}
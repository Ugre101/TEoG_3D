using Cinemachine;
using Movement.ECM2.Source.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.CameraStuff
{
    public class FirstPersonCamera : CinemachineInputProvider
    {
#if Calls_Code_I_Dont_Own
        [SerializeField] ThirdPersonEcm2Character mover;
#endif
        [SerializeField] FreePlayCameraController thirdPerson;
        [SerializeField] CinemachineVirtualCamera brain;
        [SerializeField] float crounchFactor;
        [SerializeField, HideInInspector] float orgZoom;
        [SerializeField, HideInInspector] Vector3 orgCameraHeight;
# if UNITY_EDITOR
        private void OnValidate()
        {
            if (brain == null && TryGetComponent(out CinemachineVirtualCamera cam))
                brain = cam;
            orgZoom = brain.m_Lens.FieldOfView;
            orgCameraHeight = brain.Follow.localPosition;
        }
#endif
        private void OnEnable()
        {
          //  print(transform.localEulerAngles);
          //  print(thirdPerson.transform.localEulerAngles);
            brain.ForceCameraPosition(transform.position,thirdPerson.transform.rotation);
            Cursor.lockState = CursorLockMode.Locked;
#if Calls_Code_I_Dont_Own
            
            mover.SetRotationMode(RotationMode.OrientToCameraViewDirection);
            mover.SetFirstPersonMode(true);
            mover.Crouched += Crouched;
            mover.Uncrouched += UnCrouched;
            mover.MovementModeChanged += IfSwimming;
#endif
            UnCrouched();
        }

        void IfSwimming(MovementMode prevmovementmode, int prevcustommode)
        {
#if Calls_Code_I_Dont_Own
            if (mover.IsSwimming())
                ExitFirstPerson();
#endif
        }

        protected override void OnDisable()
        {
            base.OnDisable();
#if Calls_Code_I_Dont_Own
            mover.Crouched -= Crouched;
            mover.Uncrouched -= UnCrouched;
            mover.MovementModeChanged -= IfSwimming;
#endif
        }

        public override float GetAxisValue(int axis)
        {
            var action = ResolveForPlayer(axis, axis == 2 ? ZAxis : XYAxis);
            if (action == null) return 0;
            return axis switch
            {
                0 => action.ReadValue<Vector2>().x * FirstPersonCameraSettings.Sensitivity,
                1 => action.ReadValue<Vector2>().y * FirstPersonCameraSettings.Sensitivity,
                2 => action.ReadValue<float>() * FirstPersonCameraSettings.Sensitivity,
                _ => 0,
            };
        }

        private void UnCrouched() => brain.Follow.localPosition = orgCameraHeight;

        private void Crouched() => brain.Follow.localPosition /= crounchFactor;

        public void OnScrollOut(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || ctx.ReadValue<float>() >= 0)
                return;
            ExitFirstPerson();
        }

        void ExitFirstPerson()
        {
            gameObject.SetActive(false);
            thirdPerson.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            if (zoomIn)
                ZoomIn();
            else if (zoomOut)
                ZoomOut();
        }


        private void ZoomIn()
        {
            if (brain.m_Lens.FieldOfView > zoomTo)
            {
                brain.m_Lens.FieldOfView -= zoomRate * Time.deltaTime;
                return;
            }
            brain.m_Lens.FieldOfView = zoomTo;
            zoomIn = false;
        }
        private void ZoomOut()
        {
            if (brain.m_Lens.FieldOfView < orgZoom)
            {
                brain.m_Lens.FieldOfView += zoomRate * 3f * Time.deltaTime;
                return;
            }
            brain.m_Lens.FieldOfView = orgZoom;
            zoomOut = false;
        }

        [SerializeField, Range(5f, 45f)] float zoomTo;
        [SerializeField, Range(10f, 25f)] float zoomRate;
        bool zoomIn;
        bool zoomOut;

        public void Binoculars(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                zoomIn = true;
                zoomOut = false;
            }
            else if (ctx.canceled)
            {
                zoomIn = false;
                zoomOut = true;
            }
        }

    }
}

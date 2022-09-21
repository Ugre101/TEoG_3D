using AvatarStuff;
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
        [SerializeField,Range(1.5f,2.5f)] float crounchFactor;
        [SerializeField, HideInInspector,] float orgZoom;
        [SerializeField, HideInInspector,] Vector3 orgCameraHeight;

        [SerializeField, Range(5f, 45f),] float zoomTo;
        [SerializeField, Range(10f, 25f),] float zoomRate;
        [SerializeField,Header("Scale clipping")] AvatarScaler avatarScaler;
        [SerializeField, Range(0.1f,0.5f),] float nearClipping = 0.5f;
        bool zoomIn;
        bool zoomOut;

        void Update()
        {
            if (zoomIn)
                ZoomIn();
            else if (zoomOut)
                ZoomOut();
        }

        void OnEnable()
        {
            //  print(transform.localEulerAngles);
            //  print(thirdPerson.transform.localEulerAngles);
            brain.ForceCameraPosition(transform.position, thirdPerson.transform.rotation);
            Cursor.lockState = CursorLockMode.Locked;
#if Calls_Code_I_Dont_Own

            mover.SetRotationMode(RotationMode.OrientToCameraViewDirection);
            mover.SetFirstPersonMode(true);
            mover.Crouched += Crouched;
            mover.Uncrouched += UnCrouched;
            mover.MovementModeChanged += IfSwimming;
#endif
            avatarScaler.SizeChange += AvatarScalerOnSizeChange;
            UnCrouched();
            AvatarScalerOnSizeChange(avatarScaler.Height);
        }

        void AvatarScalerOnSizeChange(float obj)
        {
            float clip = obj * nearClipping;
            brain.m_Lens.NearClipPlane = clip;
        }

    

        protected override void OnDisable()
        {
            base.OnDisable();
#if Calls_Code_I_Dont_Own
            mover.Crouched -= Crouched;
            mover.Uncrouched -= UnCrouched;
            mover.MovementModeChanged -= IfSwimming;
#endif
            avatarScaler.SizeChange -= AvatarScalerOnSizeChange;
        }
# if UNITY_EDITOR
        void OnValidate()
        {
            if (brain == null && TryGetComponent(out CinemachineVirtualCamera cam))
                brain = cam;
            orgZoom = brain.m_Lens.FieldOfView;
            orgCameraHeight = brain.Follow.localPosition;
        }
#endif

        void IfSwimming(MovementMode prevmovementmode, int prevcustommode)
        {
#if Calls_Code_I_Dont_Own
            if (mover.IsSwimming())
                ExitFirstPerson();
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

        void UnCrouched() => brain.Follow.localPosition = orgCameraHeight;

        void Crouched() => brain.Follow.localPosition /= crounchFactor;

        public void OnScrollOut(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            var value = ctx.ReadValue<float>();
            if (value < 0)
                ExitFirstPerson();
        }

        void ExitFirstPerson()
        {
            gameObject.SetActive(false);
            thirdPerson.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }


        void ZoomIn()
        {
            if (brain.m_Lens.FieldOfView > zoomTo)
            {
                brain.m_Lens.FieldOfView -= zoomRate * Time.deltaTime;
                return;
            }

            brain.m_Lens.FieldOfView = zoomTo;
            zoomIn = false;
        }

        void ZoomOut()
        {
            if (brain.m_Lens.FieldOfView < orgZoom)
            {
                brain.m_Lens.FieldOfView += zoomRate * 3f * Time.deltaTime;
                return;
            }

            brain.m_Lens.FieldOfView = orgZoom;
            zoomOut = false;
        }

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
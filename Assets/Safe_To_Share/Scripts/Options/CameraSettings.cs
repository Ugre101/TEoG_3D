

using Cinemachine;
using UnityEngine;

namespace Options
{
    public class CameraSettings : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        const string SaveName = "CameraRenderDistance";
        static float? renderDistance;
    
        static CinemachineVirtualCamera cam;

        public static float RenderDistance
        {
            get
            {
                renderDistance ??= PlayerPrefs.GetFloat(SaveName, 1000f);
                return renderDistance.Value;
            }
            set
            {
                renderDistance = value;
                PlayerPrefs.SetFloat(SaveName,value);
                if (cam != null)
                    cam.m_Lens.FarClipPlane = value;
            }
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            if (virtualCamera == null)
            {
                if (!TryGetComponent(out CinemachineVirtualCamera gotCam))
                    return;
                virtualCamera = gotCam;
            }
            cam = virtualCamera;
            virtualCamera.m_Lens.FarClipPlane = RenderDistance;
        }
    }
}

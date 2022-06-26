using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class RotateTowardsCamera : MonoBehaviour
    {
        void Start()
        {
            if (Camera.main is { }) 
                FaceCamera(Camera.main.transform);
        }


        void FaceCamera(Transform cam)
        {
            Transform trans = transform;
            Vector3 rot = trans.eulerAngles;
            rot.x = cam.eulerAngles.x;
            trans.eulerAngles = rot;
        }
    }
}
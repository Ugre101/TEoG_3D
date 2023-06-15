using UnityEngine;

namespace Safe_To_Share.Scripts {
    public sealed class RotateTowardsCamera : MonoBehaviour {
        void Start() {
            if (Camera.main is not null)
                FaceCamera(Camera.main.transform);
        }


        void FaceCamera(Transform cam) {
            var trans = transform;
            var rot = trans.eulerAngles;
            rot.x = cam.eulerAngles.x;
            trans.eulerAngles = rot;
        }
    }
}
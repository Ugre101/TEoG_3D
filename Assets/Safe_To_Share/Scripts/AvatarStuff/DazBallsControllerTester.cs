using UnityEngine;

namespace AvatarStuff {
    public sealed class DazBallsControllerTester : MonoBehaviour {
        [SerializeField, Range(0, 10f),] float size;
        [SerializeField] bool hidden;

        void OnValidate() {
            if (!TryGetComponent(out DazBallsController ballsController))
                return;
            ballsController.ReSize(size);
            ballsController.ShowOrHide(!hidden);
        }
    }
}
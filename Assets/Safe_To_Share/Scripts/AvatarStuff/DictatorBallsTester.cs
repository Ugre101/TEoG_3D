using Character.Organs.OrgansContainers;
using UnityEngine;

namespace AvatarStuff
{
    public class DictatorBallsTester : MonoBehaviour
    {
        [SerializeField, Range(0, 10f),] float size;
        [SerializeField] BallsContainer fluidsCon = new();
        [SerializeField] bool hidden;
        void OnValidate()
        {
            if (!TryGetComponent(out DictatorBalls ballsController))
                return;
            ballsController.ReSize(size);
            ballsController.ShowOrHide(!hidden);
            ballsController.SetupFluidStretch(fluidsCon);
        }
    }
}
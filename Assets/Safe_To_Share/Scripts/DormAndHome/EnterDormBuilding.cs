// using DormAndHome.Dorm.UI;

using Movement.ECM2.Source.Characters;
using UnityEngine;

namespace DormAndHome
{
    public sealed class EnterDormBuilding : MonoBehaviour
    {
        //  [SerializeField] DormBuildingCanvas dormBuildingCanvas;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            //    dormBuildingCanvas.EnterBuilding();
            if (other.TryGetComponent(out ThirdPersonEcm2Character mover)) mover.Stop();
        }

        void OnTriggerExit(Collider other)
        {
            //  if (other.CompareTag("Player"))
            //    dormBuildingCanvas.LeaveBuilding();
        }
    }
}
// using DormAndHome.Dorm.UI;

using UnityEngine;

namespace DormAndHome {
    public sealed class EnterDormBuilding : MonoBehaviour {
        //  [SerializeField] DormBuildingCanvas dormBuildingCanvas;

        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            //    dormBuildingCanvas.EnterBuilding();
            if (other.TryGetComponent(out Safe_To_Share.Scripts.Movement.HoverMovement.Movement mover))
                mover.Stop();
        }

        void OnTriggerExit(Collider other) {
            //  if (other.CompareTag("Player"))
            //    dormBuildingCanvas.LeaveBuilding();
        }
    }
}
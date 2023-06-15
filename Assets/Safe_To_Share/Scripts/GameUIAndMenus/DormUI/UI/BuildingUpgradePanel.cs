using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI {
    public abstract class BuildingUpgradePanel : MonoBehaviour {
        [SerializeField] protected DormUpgradeButton mainBuilding;
        [SerializeField] protected DormUpgradeButton prefab;
        [SerializeField] protected PlayerHolder playerHolder;
        [SerializeField] protected Transform content;
    }
}
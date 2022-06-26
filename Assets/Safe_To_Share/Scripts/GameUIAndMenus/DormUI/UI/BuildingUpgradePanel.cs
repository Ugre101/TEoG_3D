using AvatarStuff.Holders;
using Character.PlayerStuff;
using UnityEngine;

namespace DormAndHome.Dorm.UI
{
    public abstract class BuildingUpgradePanel : MonoBehaviour
    {
        [SerializeField] protected DormUpgradeButton mainBuilding;
        [SerializeField] protected DormUpgradeButton prefab;
        [SerializeField] protected PlayerHolder playerHolder;
        [SerializeField] protected Transform content;
    }
}
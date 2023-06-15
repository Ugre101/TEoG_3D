using DormAndHome.Dorm;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI {
    public abstract class DormBuildingCanvas : MonoBehaviour {
        [SerializeField] protected GameObject optionPanel, upgradePanel;
        protected static DormManager Manager => DormManager.Instance;
        protected abstract bool HasBuilding { get; }

        public void EnterBuilding() {
            gameObject.SetActive(true);
            if (HasBuilding)
                OpenOptionPanel();
            else
                OpenUpgradePanel();
        }

        protected virtual void OpenUpgradePanel() => transform.AwakeChildren(optionPanel);

        protected virtual void OpenOptionPanel() => transform.AwakeChildren(upgradePanel);

        public void LeaveBuilding() {
            transform.SleepChildren();
            gameObject.SetActive(false);
        }
    }
}
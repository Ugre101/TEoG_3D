using CustomClasses;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI {
    public abstract class DormSleepAreaShared : MonoBehaviour, ICancelMeBeforeOpenPauseMenu {
        [SerializeField] protected GameObject dormPanel;
        [SerializeField] protected GameObject upgradePanel;


        public bool BlockIfActive() {
            if (!gameObject.activeSelf) return false;
            Leave();
            return true;
        }

        public virtual void Enter() {
            GameManager.Pause();
            gameObject.SetActive(true);
        }

        public virtual void Leave() {
            GameManager.Resume(false);
            gameObject.SetActive(false);
            transform.SleepChildren();
        }
    }
}
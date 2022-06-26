using Assets.Scripts.Static;
using CustomClasses;
using GameUIAndMenus;
using UnityEngine;

namespace DormAndHome.Dorm.UI
{
    public abstract class DormSleepAreaShared : MonoBehaviour, ICancelMeBeforeOpenPauseMenu, IBlockGameUI
    {
        [SerializeField] protected GameObject dormPanel;
        [SerializeField] protected GameObject upgradePanel;

        public bool Block => dormPanel.activeInHierarchy || upgradePanel.activeInHierarchy;

        public bool BlockIfActive()
        {
            if (dormPanel.activeInHierarchy || upgradePanel.activeInHierarchy)
            {
                Leave();
                return true;
            }

            return false;
        }

        public virtual void Enter()
        {
            GameManager.Pause();
            gameObject.SetActive(true);
        }

        public virtual void Leave()
        {
            GameManager.Resume(false);
            transform.SleepChildren();
        }
    }
}
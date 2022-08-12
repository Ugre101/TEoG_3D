using CustomClasses;
using GameUIAndMenus;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace DormAndHome.Dorm.UI
{
    public abstract class DormSleepAreaShared : MonoBehaviour, ICancelMeBeforeOpenPauseMenu, IBlockGameUI
    {
        [SerializeField] protected GameObject dormPanel;
        [SerializeField] protected GameObject upgradePanel;


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
            GameUIManager.BlockList.Add(this);
        }

        public virtual void Leave()
        {
            GameManager.Resume(false);
            transform.SleepChildren();
            GameUIManager.BlockList.Remove(this);
        }
    }
}
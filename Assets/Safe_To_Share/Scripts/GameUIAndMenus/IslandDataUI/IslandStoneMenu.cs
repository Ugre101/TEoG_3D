using CustomClasses;
using UnityEngine;

namespace GameUIAndMenus.IslandDataUI
{
    public class IslandStoneMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu, IBlockGameUI
    {
        public bool Block => gameObject.activeInHierarchy;

        public bool BlockIfActive()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }
    }
}
using Character.PlayerStuff;
using CustomClasses;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public abstract class GameMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField, HideInInspector,] protected PlayerHolder holder;
        [SerializeField, HideInInspector,] protected GameCanvas gameCanvas;
        protected Player Player => holder.Player;

        public virtual bool BlockIfActive()
        {
            if (gameObject == null || !gameObject.activeInHierarchy) return false;
            gameObject.SetActive(false);
            gameCanvas.CloseMenus();
            return true;

        }

        public virtual void SetPlayer(PlayerHolder value, GameCanvas canvas)
        {
            holder = value;
            gameCanvas = canvas;
        }

        public virtual void ManualSceneChangeDestroy()
        {
        }
    }
}
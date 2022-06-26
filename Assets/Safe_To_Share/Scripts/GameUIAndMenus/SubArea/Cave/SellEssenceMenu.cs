using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Currency;
using CustomClasses;
using GameUIAndMenus;
using UnityEngine;

namespace SubArea.Cave
{
    public class SellEssenceMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] SellEssenceSlider sellFemi, sellMasc;

        void OnEnable() => CanvasPauseMenu.CancelMe.Add(this);

        void OnDisable() => CanvasPauseMenu.CancelMe.Remove(this);

        void OnDestroy() => CanvasPauseMenu.CancelMe.Remove(this);

        public bool BlockIfActive()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }

        public void Setup(Player player)
        {
            gameObject.SetActive(true);
            sellFemi.Setup(player.Essence.Femininity, PlayerGold.GoldBag);
            sellMasc.Setup(player.Essence.Masculinity, PlayerGold.GoldBag);
        }
    }
}
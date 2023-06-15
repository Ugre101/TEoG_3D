using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.SubArea.Cave {
    public sealed class SellEssenceMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu {
        [SerializeField] SellEssenceSlider sellFemi, sellMasc;

        void OnEnable() => CanvasPauseMenu.CancelMe.Add(this);

        void OnDisable() => CanvasPauseMenu.CancelMe.Remove(this);

        void OnDestroy() => CanvasPauseMenu.CancelMe.Remove(this);

        public bool BlockIfActive() {
            if (!gameObject.activeInHierarchy) return false;
            gameObject.SetActive(false);
            return true;
        }

        public void Setup(Player player) {
            gameObject.SetActive(true);
            sellFemi.Setup(player.Essence.Femininity, PlayerGold.GoldBag);
            sellMasc.Setup(player.Essence.Masculinity, PlayerGold.GoldBag);
        }
    }
}
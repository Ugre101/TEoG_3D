using Character.Ailments;
using Character.PlayerStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.EffectUI {
    public sealed class TiredEffectIcon : EffectIcon {
        [SerializeField] TextMeshProUGUI sleepText;
        protected override string HoverText => "Sleepy";

        public void Setup(Player player) {
            if (Tired.Has(player)) {
                gameObject.SetActive(true);
                sleepText.color = Color.yellow;
            } else if (DeadTired.Has(player)) {
                gameObject.SetActive(true);
                sleepText.color = Color.red;
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}
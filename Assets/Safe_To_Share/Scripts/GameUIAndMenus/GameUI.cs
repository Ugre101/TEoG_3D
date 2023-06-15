using Character.StatsStuff.HealthStuff.UI;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class GameUI : GameMenu {
        [SerializeField] HealthSlider hp, wp;

        void OnEnable() {
            Refresh();
            holder.RePlaced += Refresh;
        }

        void OnDisable() => holder.RePlaced += Refresh;

        public override bool BlockIfActive() => false;


        void Refresh() {
            var stats = Player.Stats;
            wp.Setup(stats.WillPower);
            hp.Setup(stats.Health);
        }
    }
}
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using SaveStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Shrines {
    public class ShimeraShrine : BaseShrine {
        [SerializeField] Transform content;
        [SerializeField] SacrificeARaceEssence button;
        [SerializeField] AreYouSure areYouSure;

        protected override ShrinePoints Points => ShrinePointsManager.ChimeraShrine;

        public override void EnterShrine(Player player) {
            base.EnterShrine(player);
            content.KillChildren();
            foreach (var raceEssence in player.RaceSystem.AllRaceEssence) {
                var btn= Instantiate(button, content);
                btn.Setup(raceEssence, player,areYouSure);
            }
        }
    }
}
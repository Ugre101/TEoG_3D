using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI {
    public sealed class IslandStoneCanvas : MonoBehaviour {
        [SerializeField] IslandStone islandStone;
        [SerializeField] IslandStoneMenu menu;

        public void CLoseMenus() => transform.SleepChildren();

        public void Open(Player player) => menu.Open(player);
    }
}
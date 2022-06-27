using Character.IslandData;
using Character.PlayerStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public abstract class IslandStoneOption : MonoBehaviour
    {
        [SerializeField] protected Player player;
        [SerializeField] protected Islands island;
        [SerializeField] protected TextMeshProUGUI currentAmount;

        public void Setup(Player player) => this.player = player;

        public abstract void Click();
    }
}
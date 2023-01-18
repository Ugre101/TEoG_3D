using Character.IslandData;
using Character.PlayerStuff;
using Items;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public abstract class IslandStoneOption : MonoBehaviour
    {
        [SerializeField] protected Player player;
        [SerializeField] protected Islands island;
        [SerializeField] protected TextMeshProUGUI currentAmount;

        protected string emptyGuid;
        public void Setup(Player player, string opResult)
        {
            this.player = player;
            emptyGuid = opResult;
        }

        public abstract void IncreaseClick();
        public abstract void DecreaseClick();
    }
}
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

        protected string EmptyGuid;
        public virtual void Setup(Player parPlayer, string opResult)
        {
            player = parPlayer;
            EmptyGuid = opResult;
        }

        public abstract void IncreaseClick();
        public abstract void DecreaseClick();
    }
}
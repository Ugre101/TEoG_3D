using System;
using AvatarStuff.Holders;
using Character.StatsStuff;
using Safe_To_Share.Scripts.Holders;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUIAndMenus.Menus.Level
{
    [RequireComponent(typeof(Button))]
    public class StatButton : BaseLevelButton, IPointerEnterHandler
    {
        [SerializeField] Color statColor = Color.white;
        [SerializeField] CharStatType statType;
        [SerializeField] int cost = 1;
        [SerializeField] TextMeshProUGUI currentAmount;

# if UNITY_EDITOR
        void OnValidate()
        {
            rune.color = statColor;
            btnText.text = nameof(statType);
        }
#endif
        public void OnPointerEnter(PointerEventData eventData) =>
            ShowStatInfo?.Invoke(statType, cost, transform.position);

        public override void Setup(PlayerHolder player)
        {
            this.PlayerHolder = player;
            UpdateValue();
        }


        public static event Action<CharStatType, int, Vector3> ShowStatInfo;

        void UpdateValue()
        {
            if (PlayerHolder.Player.Stats.GetCharStats.TryGetValue(statType, out CharStat stat))
                currentAmount.text = stat.BaseValue.ToString();
        }

        protected override void CanAfford(int obj) => Afford = cost <= obj;

        protected override void OnClick()
        {
            if (!PlayerHolder.Player.LevelSystem.TryUsePoints(cost))
                return;
            PlayerHolder.Player.Stats.GetCharStats[statType].BaseValue++;
            UpdateValue();
        }
    }
}
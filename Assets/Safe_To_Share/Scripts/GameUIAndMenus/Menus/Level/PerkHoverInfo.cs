using System;
using Character.StatsStuff;
using Safe_To_Share.Scripts.GameUIAndMenus.Menus.EssenceMenu;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level
{
    public sealed class PerkHoverInfo : BaseHoverInfo
    {
        protected override void Start()
        {
            EssencePerkButton.ShowPerkInfo += ShowPerkInfo;
            PerkButton.ShowPerkInfo += ShowPerkInfo;
            BaseLevelButton.StopShowPerkInfo += StopShow;
            StatButton.ShowStatInfo += ShowStatInfo;
            base.Start();
        }

        void OnDestroy()
        {
            EssencePerkButton.ShowPerkInfo -= ShowPerkInfo;
            PerkButton.ShowPerkInfo -= ShowPerkInfo;
            BaseLevelButton.StopShowPerkInfo -= StopShow;
            StatButton.ShowStatInfo -= ShowStatInfo;
        }

        void ShowStatInfo(CharStatType charStatType, int statCost, Vector3 pos)
        {
            SetPos(pos);
            title.text = nameof(charStatType);
            cost.text = $"Cost {{{statCost}}}";
            desc.text = GetDesc();
            needPerk.text = string.Empty;
            exclusivePerk.text = string.Empty;
            gameObject.SetActive(true);

            string GetDesc() => charStatType switch
            {
                CharStatType.Strength => "Increases hit damage",
                CharStatType.Intelligence => "Increases willpower",
                CharStatType.Constitution => "Increases health",
                CharStatType.Charisma => "Increases tease damage",
                CharStatType.Agility => "Increases speed allowing you to hit before enemy",
                _ => throw new ArgumentOutOfRangeException(nameof(charStatType), charStatType, null),
            };
        }
    }
}
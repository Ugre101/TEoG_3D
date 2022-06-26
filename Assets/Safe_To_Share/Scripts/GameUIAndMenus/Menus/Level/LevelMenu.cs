using System.Linq;
using Character.LevelStuff;
using UnityEngine;

namespace GameUIAndMenus.Menus.Level
{
    public class LevelMenu : BaseLevelMenu
    {
        [SerializeField] StartPerkIcon startPerkIcon;

        protected override void OnEnable()
        {
            base.OnEnable();
            Player.LevelSystem.PerkPointsChanged += UpdatePerkPoints;
            Setup();
        }

        void OnDisable()
        {
            Player.LevelSystem.PerkPointsChanged -= UpdatePerkPoints;
        }

        void Setup()
        {
            UpdatePerkPoints(Player.LevelSystem.Points);
            ShowStartPerk();
        }

        void ShowStartPerk()
        {
            BasicPerk startPerk = Player.LevelSystem.OwnedPerks.FirstOrDefault(p => p.PerkType == PerkType.StartPerk);
            if (startPerk != null)
                startPerkIcon.Setup(startPerk);
        }


        void UpdatePerkPoints(int value) => perkPointsLeft.text = $"Perk points {value}";
    }
}
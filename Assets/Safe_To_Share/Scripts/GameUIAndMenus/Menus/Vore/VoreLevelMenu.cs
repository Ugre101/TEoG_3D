using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public class VoreLevelMenu : BaseLevelMenu
    {
        int VorePoints => Player.Vore.Level.Points;

        protected override void OnEnable()
        {
            base.OnEnable();
            Player.Vore.Level.PerkPointsChanged += UpdatePerkPoints;
            UpdatePerkPoints(Player.Vore.Level.Points);
        }

        void OnDisable() => Player.Vore.Level.PerkPointsChanged -= UpdatePerkPoints;

        void UpdatePerkPoints(int value) => perkPointsLeft.text = $"Perk points {value}";
    }
}
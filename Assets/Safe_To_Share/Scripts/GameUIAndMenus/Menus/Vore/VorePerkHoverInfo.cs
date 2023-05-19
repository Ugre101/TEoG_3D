using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public sealed class VorePerkHoverInfo : BaseHoverInfo
    {
        protected override void Start()
        {
            VorePerkButton.ShowPerkInfo += ShowPerkInfo;
            BaseLevelButton.StopShowPerkInfo += StopShow;
            base.Start();
        }

        void OnDestroy()
        {
            VorePerkButton.ShowPerkInfo -= ShowPerkInfo;
            BaseLevelButton.StopShowPerkInfo -= StopShow;
        }
    }
}
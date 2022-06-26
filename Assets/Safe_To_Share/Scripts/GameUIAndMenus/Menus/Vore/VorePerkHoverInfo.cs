using GameUIAndMenus.Menus.Level;

namespace GameUIAndMenus.Menus.Vore
{
    public class VorePerkHoverInfo : BaseHoverInfo
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
using Character.EssenceStuff;
using Character.EssenceStuff.UI;
using Character.Organs.UI;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.Menus.EssenceMenu
{
    public class EssenceMenu : GameMenu
    {
        [SerializeField] EssenceBars essenceBars;
        [SerializeField] TextMeshProUGUI stableAmount, giveAmount, drainAmount;
        [SerializeField] ManualOrganGrowth dick, balls, vagina, boobs;
        [SerializeField] EssenceOptionsToggles essenceOptions;

        void OnEnable()
        {
            ManualOrganGrowth.Change = false;
            GrowOrganButton.Change = false;
            EssenceSystem essence = Player.Essence;
            essenceBars.Setup(essence);
            stableAmount.text = $"Stable amount\n{essence.StableEssence.Value}";
            giveAmount.text = $"Give amount:\n{essence.GiveAmount.Value}";
            drainAmount.text = $"Drain amount\n{essence.DrainAmount.Value}";
            dick.Setup(Player);
            balls.Setup(Player);
            vagina.Setup(Player);
            boobs.Setup(Player);
            essenceOptions.Setup(Player);
        }


        void OnDisable()
        {
            if (ManualOrganGrowth.Change || GrowOrganButton.Change)
                holder.UpdateAvatar();
        }
    }
}
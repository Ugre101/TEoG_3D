using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public class VoreStartMenu : GameMenu
    {
        [SerializeField] GameObject drainChooseContainer;
        [SerializeField] TMP_Dropdown drainChoose;

        void OnEnable() => SetupDrain();

        void SetupDrain()
        {
            if (Player.Vore.orgasmDrain.Value <= 0)
            {
                drainChooseContainer.gameObject.SetActive(false);
                return;
            }

            drainChooseContainer.gameObject.SetActive(true);

            drainChoose.SetupTmpDropDown(Player.Vore.DrainEssenceType, ChangeDrainOption);
        }

        void ChangeDrainOption(int arg0) =>
            Player.Vore.DrainEssenceType = UgreTools.IntToEnum(arg0, Player.Vore.DrainEssenceType);
    }
}
using System.Collections.Generic;
using DormAndHome.Dorm.Buildings;
using Safe_To_Share.Scripts.Static;
using TMPro;

namespace DormAndHome.Dorm.UI
{
    public class KitchenOptionsPanel : BuildingOptionPanel
    {
        static DormKitchen Kitchen => DormManager.Instance.Buildings.Kitchen;

        public override void Setup()
        {
            List<TMP_Dropdown.OptionData> options = new()
            {
                CreateDietOption(DormKitchen.FeedMode.Nothing),
            };
            int kitchenLevel = Kitchen.Level;
            if (kitchenLevel > 0)
            {
                options.Add(CreateDietOption(DormKitchen.FeedMode.Diet));
                options.Add(CreateDietOption(DormKitchen.FeedMode.Normal));
            }

            if (kitchenLevel > 1)
            {
                options.Add(CreateDietOption(DormKitchen.FeedMode.Bulk));
                options.Add(CreateDietOption(DormKitchen.FeedMode.Fatten));
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.value = Kitchen.DietMode.IndexOfEnum();
            dropdown.onValueChanged.AddListener(ChangeDietMode);
        }

        static void ChangeDietMode(int arg0) =>
            Kitchen.DietMode = UgreTools.IntToEnum(arg0, DormKitchen.FeedMode.Nothing);

        static TMP_Dropdown.OptionData CreateDietOption(DormKitchen.FeedMode dietMode) => new(dietMode.ToString());
    }
}
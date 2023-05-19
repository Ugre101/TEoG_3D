using System.Collections.Generic;
using DormAndHome.Dorm;
using DormAndHome.Dorm.Buildings;
using Safe_To_Share.Scripts.Static;
using TMPro;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public sealed class GymOptionsPanel : BuildingOptionPanel
    {
        static DormGym Gym => DormManager.Instance.Buildings.Gym;

        public override void Setup()
        {
            List<TMP_Dropdown.OptionData> options = new()
            {
                CreateGymOption(DormGym.TrainMode.None),
            };
            int gymLevel = Gym.Level;
            if (gymLevel > 0)
            {
                options.Add(CreateGymOption(DormGym.TrainMode.Cardio));
                options.Add(CreateGymOption(DormGym.TrainMode.LightBodyBuilding));
            }

            if (gymLevel > 1)
            {
                options.Add(CreateGymOption(DormGym.TrainMode.Mixed));
                options.Add(CreateGymOption(DormGym.TrainMode.BodyBuilding));
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.value = Gym.TrainSchema.IndexOfEnum();
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener(ChangeTrainMode);
        }

        static void ChangeTrainMode(int arg0) => Gym.TrainSchema = UgreTools.IntToEnum(arg0, DormGym.TrainMode.None);

        static TMP_Dropdown.OptionData CreateGymOption(DormGym.TrainMode trainMode) => new(nameof(trainMode));
    }
}
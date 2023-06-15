using System;
using System.Collections.Generic;
using Character.BodyStuff;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings {
    [Serializable]
    public class DormKitchen : Building {
        public enum FeedMode {
            Nothing,
            Diet,
            Normal,
            Bulk,
            Fatten,
        }

        [SerializeField] FeedMode feedMode;

        protected override int[] UpgradeCosts { get; } = { 100, 300, };

        public FeedMode DietMode {
            get => feedMode;
            set => feedMode = value;
        }


        public override void TickBuildingEffect(List<DormMate> dormMates) {
            if (Level < 1)
                return;
            foreach (var mate in dormMates)
                switch (DietMode) {
                    case FeedMode.Nothing:
                        break;
                    case FeedMode.Diet:
                        if (mate.Body.GetFatRatio() <= 0.8f)
                            BodyExtensions.GainPercentFat(mate.Body, 0.01f);
                        break;
                    case FeedMode.Normal:
                        if (mate.Body.GetFatRatio() <= 1f)
                            BodyExtensions.GainPercentFat(mate.Body, 0.01f);
                        break;
                    case FeedMode.Bulk:
                        if (mate.Body.GetFatRatio() <= 1.2f)
                            BodyExtensions.GainPercentFat(mate.Body, 0.02f);
                        break;
                    case FeedMode.Fatten:
                        if (mate.Body.GetFatRatio() <= 1.5f)
                            BodyExtensions.GainPercentFat(mate.Body, 0.05f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }
    }
}
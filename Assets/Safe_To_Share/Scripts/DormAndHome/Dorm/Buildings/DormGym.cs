using System;
using System.Collections.Generic;
using Character.BodyStuff;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings
{
    [Serializable]
    public class DormGym : Building
    {
        public enum TrainMode
        {
            None,
            Cardio,
            Mixed,
            LightBodyBuilding,
            BodyBuilding,
        }

        [SerializeField] TrainMode trainMode;

        protected override int[] UpgradeCosts { get; } = { 200, 500, };

        public TrainMode TrainSchema
        {
            get => trainMode;
            set => trainMode = value;
        }

        public override void TickBuildingEffect(List<DormMate> dormMates)
        {
            if (Level < 1)
                return;
            foreach (DormMate mate in dormMates)
            {
                Body mateBody = mate.Body;
                switch (TrainSchema)
                {
                    case TrainMode.None:
                        break;
                    case TrainMode.Cardio:
                        mateBody.BurnFatHour(2);
                        break;
                    case TrainMode.Mixed:
                        mateBody.BurnFatHour(1, 0.5f);
                        BodyExtensions.Train(mateBody);
                        break;
                    case TrainMode.LightBodyBuilding:
                        mateBody.BurnFatHour();
                        BodyExtensions.Train(mateBody, 1.2f);
                        break;
                    case TrainMode.BodyBuilding:
                        mateBody.BurnFatHour();
                        BodyExtensions.Train(mateBody, 1.4f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
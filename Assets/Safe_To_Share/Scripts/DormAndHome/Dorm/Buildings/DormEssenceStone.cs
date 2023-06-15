using System;
using System.Collections.Generic;
using Character.EssenceStuff;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings {
    [Serializable]
    public class DormEssenceStone : Building {
        [SerializeField] DrainEssenceType gainType;

        protected override int[] UpgradeCosts { get; } = { 250, 500, };

        public DrainEssenceType GainType {
            get => gainType;
            set => gainType = value;
        }

        public override void TickBuildingEffect(List<DormMate> dormMates) {
            if (Level <= 0)
                return;

            switch (GainType) {
                case DrainEssenceType.None:
                    break;
                case DrainEssenceType.Masc:
                    foreach (var dormMate in dormMates)
                        dormMate.GainMasc(Level * 2);
                    break;
                case DrainEssenceType.Femi:
                    foreach (var dormMate in dormMates)
                        dormMate.GainFemi(Level * 2);
                    break;
                case DrainEssenceType.Both:
                    foreach (var dormMate in dormMates) {
                        dormMate.GainMasc(Level);
                        dormMate.GainFemi(Level);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
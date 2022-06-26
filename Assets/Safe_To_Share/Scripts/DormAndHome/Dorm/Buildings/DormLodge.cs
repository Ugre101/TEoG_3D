using System;
using System.Collections.Generic;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings
{
    [Serializable]
    public class DormLodge : Building
    {
        [SerializeField] DormEssenceStone essenceStone = new();

        protected override int[] UpgradeCosts { get; } = { 100, 300, 700, 1500, };

        public DormEssenceStone EssenceStone => essenceStone;

        public override void TickBuildingEffect(List<DormMate> dormMates)
        {
        }
    }
}
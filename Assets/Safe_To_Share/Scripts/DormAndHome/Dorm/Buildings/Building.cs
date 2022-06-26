using System;
using System.Collections.Generic;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings
{
    [Serializable]
    public abstract class Building
    {
        [SerializeField, Min(0),] int level;

        public int Level => level;
        protected abstract int[] UpgradeCosts { get; }

        public bool CanUpgrade => Level < UpgradeCosts.Length;

        public int UpgradeCost
        {
            get
            {
                if (UpgradeCosts.Length > Level)
                    return UpgradeCosts[Level];
                throw new IndexOutOfRangeException();
            }
        }

        public void Upgrade()
        {
            level++;
            Upgraded?.Invoke();
        }

        public event Action Upgraded;
        
        public abstract void TickBuildingEffect(List<DormMate> dormMates);
    }
}
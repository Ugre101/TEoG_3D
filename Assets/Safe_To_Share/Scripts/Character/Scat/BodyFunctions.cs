using System;
using Character;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Scat
{
    [Serializable]
    public class BodyFunctions : ITickHour
    {
        [field: SerializeField] public float HydrationLevel { get; private set; } = 5f;
        [field: SerializeField] public BaseConstIntStat MaxHydration { get; private set; } = new(10);
        [field: SerializeField] public Bladder Bladder { get; private set; } = new();

        public bool TickHour(int ticks = 1)
        {
            switch (HydrationLevel)
            {
                case > 0.2f:
                    Bladder.Fill(0.1f);
                    DecreaseHydrationLevel(0.2f);
                    break;
                case > 0:
                    Bladder.Fill(HydrationLevel / 2);
                    HydrationLevel = 0;
                    break;
            }

            return false;
        }

        public void FullHydration()
        {
            HydrationLevel = MaxHydration.Value;
        }

        public void IncreaseHydrationLevel(float by)
        {
            if (HydrationLevel + by > MaxHydration.Value)
            {
                // Overflow
                var overflow = HydrationLevel + by - MaxHydration.Value;
                Bladder.Fill(overflow);
            }

            HydrationLevel = Mathf.Clamp(HydrationLevel + by, 0, MaxHydration.Value);
        }

        public void DecreaseHydrationLevel(float by)
        {
            HydrationLevel = Mathf.Clamp(HydrationLevel - by, 0, MaxHydration.Value);
        }
    }
}
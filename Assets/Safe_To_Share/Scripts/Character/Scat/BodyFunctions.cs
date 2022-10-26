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
        public float IncreaseHydrationLevel(float by)
        {
            float orgHyd = HydrationLevel;
            HydrationLevel = Mathf.Min(HydrationLevel + by, MaxHydration.Value);
            return 0;
        }

        public bool TickHour(int ticks = 1)
        {
            HydrationLevel -= 0.2f;
            return false;
        }
    }

    public static class FoodAndDrinkExtensions
    {
        public static void Drink(this BaseCharacter character, int amount)
        {
            
            // 10 per kg
        }

        public static void TickFoodAndDrink(this BaseCharacter character, int ticks = 1)
        {
            
        }
    }
}
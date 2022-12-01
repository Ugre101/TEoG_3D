using System;
using Character;
using Character.Ailments;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Scat
{
    public static class FoodAndDrinkExtensions
    {
        public static void Drink(this BaseCharacter character, Amount amount)
        {
            // var by = amount / (character.Body.Weight / 2);
            var by = amount switch
            {
                Amount.Small => 0.5f,
                Amount.Medium => 2f,
                Amount.Large => 5f,
                Amount.Huge => 10f,
                _ => throw new ArgumentOutOfRangeException(nameof(amount), amount, null)
            };
            character.BodyFunctions.IncreaseHydrationLevel(by);
        }

        // 10 per kg
        public enum Amount
        {
            Small,
            Medium,
            Large,
            Huge,
        }
        public static void Eat(this BaseCharacter character,int kcal, float foodQuality = 100)
        {
            character.Body.Fat.BaseValue += kcal / 9000f; // Might change to account for height
            character.CheckHungry();
            float p = kcal / 2f / foodQuality; // Better quality less p
            character.SexualOrgans.Anals.Fluid.IncreaseCurrentValue(p);
        }
        public static void TickFoodAndDrink(this BaseCharacter character, int ticks = 1)
        {
        }
    }
}
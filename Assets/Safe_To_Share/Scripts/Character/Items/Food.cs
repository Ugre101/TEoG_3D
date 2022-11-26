using Character;
using Safe_To_Share.Scripts.Character.Scat;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Food", menuName = "Items/New food item")]
    public class Food : Item
    {
        [SerializeField, Min(0f),] int kcal;
        [SerializeField, Range(0f, 10f)] float reHydration;
        public override void Use(BaseCharacter user)
        {
            user.Eat(kcal);
            user.BodyFunctions.IncreaseHydrationLevel(reHydration);
            base.Use(user);
        }
    }
}
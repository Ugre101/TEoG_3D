using Character;
using Character.Ailments;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Food", menuName = "Items/New food item")]
    public class Food : Item
    {
        [SerializeField, Min(0f),] int kcal;

        public override void Use(BaseCharacter user)
        {
            user.Body.Fat.BaseValue += kcal / 9000f;
            user.CheckHungry();
            base.Use(user);
        }
    }
}
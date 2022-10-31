using Character;
using Safe_To_Share.Scripts.Character.Scat;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Food", menuName = "Items/New food item")]
    public class Food : Item
    {
        [SerializeField, Min(0f),] int kcal;

        public override void Use(BaseCharacter user)
        {
            user.Eat(kcal);
            base.Use(user);
        }
    }
}
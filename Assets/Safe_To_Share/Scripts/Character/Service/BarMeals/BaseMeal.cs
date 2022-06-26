using Character.PlayerStuff;
using Items;
using UnityEngine;

namespace Character.Service.BarMeals
{
    [CreateAssetMenu(fileName = "New Meal", menuName = "Services/Create meal", order = 0)]
    public class BaseMeal : BaseService
    {
        [SerializeField] Food foodItem;
        public override void OnUse(Player player) => foodItem.Use(player);
    }
}
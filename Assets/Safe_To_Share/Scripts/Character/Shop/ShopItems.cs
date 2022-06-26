using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Character.Shop
{
    [CreateAssetMenu(fileName = "Shop Items", menuName = "Items/Shop items", order = 0)]
    public class ShopItems : ScriptableObject
    {
        [SerializeField] List<Item> sellingItems = new();

        public List<Item> SellingItems => sellingItems;
    }
}
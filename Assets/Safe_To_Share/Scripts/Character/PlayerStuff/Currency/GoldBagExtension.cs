using System.Linq;
using Character;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Items;
using UnityEngine;

namespace Currency
{
    public static class GoldBagExtension
    {
        public static bool TryToBuy(this Player player, int cost)
        {
            int finalCost = Mathf.RoundToInt(cost * DiscountMulti(player));
            return PlayerGold.GoldBag.TryToBuy(finalCost);
        }

        public static float DiscountMulti(BaseCharacter player)
        {
            var currencyPerks = player.LevelSystem.OwnedPerks.OfType<CurrencyPerk>().ToArray();
            if (currencyPerks.Length > 0)
                return 1f - currencyPerks.Sum(cp => cp.Discount);
            return 1f;
        }

        public static bool CanAfford(this Player player, int cost)
        {
            int finalCost = Mathf.RoundToInt(cost * DiscountMulti(player));
            return PlayerGold.GoldBag.CanAfford(finalCost);
        }
        public static bool TryBuyItem(this Player player,Item item,int amount = 1)
        {
            int discountedCost = Mathf.CeilToInt(item.Value* DiscountMulti(player)  * amount);
            if (PlayerGold.GoldBag.TryToBuy(discountedCost))
            {
                player.Inventory.AddItem(item,amount);
            }
            return false;
        }
    }
}
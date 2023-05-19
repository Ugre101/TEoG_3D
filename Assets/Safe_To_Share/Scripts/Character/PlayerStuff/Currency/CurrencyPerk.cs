using Character.LevelStuff;
using UnityEngine;

namespace Currency
{
    [CreateAssetMenu(fileName = "Create CurrencyPerk", menuName = "Character/CurrencyPerk", order = 0)]
    public sealed class CurrencyPerk : BasicPerk
    {
        [SerializeField, Range(0, 100),] int discount;

        public float Discount => discount / 100f;
    }
}
using System.Globalization;
using System.Text;
using Character;
using Safe_To_Share.Scripts.Character.Scat;
using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "Food", menuName = "Items/New food item")]
    public sealed class Food : Item {
        [SerializeField, Min(0f),] int kcal;
        [SerializeField, Range(0f, 10f),] float reHydration;

        public override void Use(BaseCharacter user) {
            user.Eat(kcal);
            user.BodyFunctions.IncreaseHydrationLevel(reHydration);
            base.Use(user);
        }

        public string ExtraInfo() {
            var sb = new StringBuilder();
            if (kcal > 0) {
                sb.Append("+");
                sb.Append(kcal.ToString());
                sb.AppendLine(" Kcal");
            }

            if (reHydration > 0) {
                sb.Append("+");
                sb.Append(reHydration.ToString(CultureInfo.InvariantCulture));
                sb.Append(" Hydration");
            }

            return sb.ToString();
        }
    }
}
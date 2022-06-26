using Character.Organs;
using UnityEngine;

namespace Assets.Scripts.Character.Organs
{
    public static class BoobsConversion
    {
        static readonly string[] BraSizes =
            { "Flat", "AA", "A", "B", "C", "D", "DD", "E", "F", "I", "J", "K", "L", "M", "O", "P", "Q", "R", };

        public static string ToBraSize(this BaseOrgan boobs)
        {
            float size = boobs.Value;
            int index = Mathf.FloorToInt(size);
            return BraSizes.Length - 1 < index ? BraSizes[index] : "Enormous";
        }
    }
}
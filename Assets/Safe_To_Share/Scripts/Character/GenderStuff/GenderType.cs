using System;

namespace Character.GenderStuff {
    public enum GenderType {
        Neutral, Feminine, Masculine,
    }

    public static class GenderTypeExtensions {
        public static GenderType GetGenderType(this Gender gender) {
            switch (gender) {
                case Gender.Doll: return GenderType.Neutral;
                case Gender.Male:
                case Gender.MaleFutanari:
                case Gender.CuntBoy: return GenderType.Masculine;
                case Gender.Female:
                case Gender.DickGirl:
                case Gender.Futanari: return GenderType.Feminine;
                default: throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }
    }
}
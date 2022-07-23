using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.Static
{
    public static class MetricOrImperial
    {
        public static readonly SavedBoolSetting Metric = new("UsingImperialUnits");

        static string AddS(float value) => Mathf.FloorToInt(value) > 1f ? "s" : string.Empty;

        #region VolumeUnits

        public static string ConvertCl(this int value, bool wordAfter = true) =>
            Metric.Enabled ? ClToMetric(value, wordAfter) : ClToImperial(value, wordAfter);

        static string ClToImperial(int value, bool wordAfter = true)
        {
            float oz = value * 0.33814f;
            if (!wordAfter)
                return $"{oz:0}";
            if (value < 47)
                return $"{oz:0.##}oz";
            float pint = value * 0.02113f;
            if (value < 379)
                return pint % 1 > 0.06f
                    ? $"{Mathf.FloorToInt(pint)}pint{AddS(pint)} and {Mathf.FloorToInt(pint % 1 * 16f)}oz"
                    : $"{Mathf.FloorToInt(pint)}pint{AddS(pint)}";
            float gallon = value * 0.002642f;
            return gallon % 1 > 0.13f
                ? $"{Mathf.FloorToInt(gallon)}gallon{AddS(gallon)} and {Mathf.FloorToInt(gallon % 1 * 8f)}pints"
                : $"{Mathf.FloorToInt(gallon)}gallon{AddS(gallon)}";
        }

        static string ClToMetric(int value, bool wordAfter = true)
        {
            if (!wordAfter)
                return $"{value:0.#}";
            if (value < 100)
                return $"{value:0}cl";
            float l = value / 100f;
            return l < 10 ? $"{l:0.##}l" : $"{l:0.#}l";
        }

        public static string ConvertCl(this float value, bool wordAfter = true) =>
            Metric.Enabled ? ClToMetric(value, wordAfter) : ClToImperial(value, wordAfter);

        static string ClToImperial(float value, bool wordAfter)
        {
            float oz = value * 0.33814f;
            if (!wordAfter)
                return $"{oz:0.##}";
            if (value < 49)
                return $"{oz:0.##}oz";
            float pint = value * 0.02113f;
            if (value < 379)
                return pint % 1 > 0.06f
                    ? $"{Mathf.FloorToInt(pint)}pint{AddS(pint)} and {Mathf.RoundToInt(pint % 1 * 16f)}oz"
                    : $"{Mathf.FloorToInt(pint)}pint{AddS(pint)}";
            float gallon = value * 0.002642f;
            return gallon % 1 > 0.13f
                ? $"{Mathf.FloorToInt(gallon)}gallon{AddS(gallon)} and {Mathf.FloorToInt(gallon % 1 * 8f)}pints"
                : $"{Mathf.FloorToInt(gallon)}gallon{AddS(gallon)}";
        }

        static string ClToMetric(float value, bool wordAfter)
        {
            if (!wordAfter)
                return $"{value:0.#}";
            if (value < 1f)
                return $"{value * 10f:0.##}";
            if (value < 100)
                return $"{value:0.##}cl";
            float l = value / 100f;
            return l < 10 ? $"{l:0.##}l" : $"{l:0.#}l";
        }

        #endregion

        #region WeightUnis

        public static string ConvertKg(this int value, bool wordAfter = true) =>
            Metric.Enabled ? KgToMetric(value, wordAfter) : KgToImperial(value, wordAfter);

        static string KgToImperial(int value, bool wordAfter)
        {
            float lbs = value * 2.2046f;
            return wordAfter ? $"{lbs:0.#}lbs" : $"{lbs:0.#}";
        }

        static string KgToMetric(int value, bool wordAfter)
        {
            if (wordAfter && value > 1000)
                return $"{value / 1000:0.#} ton";
            return wordAfter ? $"{value}kg" : value.ToString();
        }

        public static string ConvertKg(this float value, bool wordAfter = true) =>
            Metric.Enabled ? KgToMetric(value, wordAfter) : KgToImperial(value, wordAfter);

        static string KgToImperial(float value, bool wordAfter)
        {
            if (wordAfter && value < 0.454f)
                return $"{value * 35.27396195:0.##}oz";
            float lbs = value * 2.2046f;
            return wordAfter ? $"{lbs:0.#}lbs" : $"{lbs:0.#}";
        }

        static string KgToMetric(float value, bool wordAfter) =>
            wordAfter switch
            {
                true when value < 1f => $"{value * 1000:0.##}g",
                true when value > 1000 => $"{value / 1000:0.#} ton",
                _ => wordAfter ? $"{value:0.#}kg" : $"{value:0.#}",
            };

        #endregion

        #region LengthUnits

        public static string ConvertCm(this int value, bool wordAfter = true) =>
            Metric.Enabled ? CmToMetric(value, wordAfter) : CmToImperial(value, wordAfter);

        static string CmToImperial(int value, bool wordAfter)
        {
            float inch = value * 0.39370079f;
            if (inch < 12f)
                return wordAfter ? $"{inch:0.##}inches" : $"{inch:0.##}";
            if (wordAfter && inch > 24)
                return Mathf.FloorToInt(inch % 12) == 0
                    ? $"{Mathf.FloorToInt(inch / 12)}feet"
                    : $"{Mathf.FloorToInt(inch / 12)}feet and {Mathf.RoundToInt(inch % 12)}inches";
            return wordAfter ? $"{inch:0}inches" : $"{inch:0}";
        }

        static string CmToMetric(int value, bool wordAfter = true)
        {
            if (wordAfter && value > 300)
                return $"{Mathf.FloorToInt(value / 100f)}m and {value % 100}cm";
            return wordAfter ? $"{value}cm" : value.ToString();
        }

        public static string ConvertCm(this float value, bool wordAfter = true) =>
            Metric.Enabled ? CmToMetric(value, wordAfter) : CmToImperial(value, wordAfter);

        static string CmToMetric(float value, bool wordAfter = true)
        {
            if (value < 30)
                return wordAfter ? $"{value:0.##}cm" : $"{value:0.##}";

            if (wordAfter && value > 300)
                return $"{Mathf.FloorToInt(value / 100f)}m and {Mathf.RoundToInt(value % 100)}cm";
            return wordAfter ? $"{value:0}cm" : $"{value:0}";
        }

        static string CmToImperial(float value, bool wordAfter = true)
        {
            float inch = value * 0.39370079f;
            if (inch < 12f)
                return wordAfter ? $"{inch:0.##}inches" : $"{inch:0.##}";
            if (wordAfter && inch > 24)
                return Mathf.FloorToInt(inch % 12) == 0
                    ? $"{Mathf.FloorToInt(inch / 12)}feet"
                    : $"{Mathf.FloorToInt(inch / 12)}feet and {Mathf.Round(inch % 12)}inches";
            return wordAfter ? $"{inch:0}inches" : $"{inch:0}";
        }

        #endregion
    }
}
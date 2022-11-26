using System.Text;
using Character.GenderStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.BodyStuff
{
    public static class BodyExtensions
    {
        public static float Train(Body body, float intensity = 1f)
        {
            BodyStat muscle = body.Muscle;
            float musclePercent = muscle.BaseValue / body.Weight;
            float demisingReturn = 1f - musclePercent;
            float gain = body.Height.BaseValue * 0.005f * demisingReturn * intensity;
            muscle.BaseValue += gain;
            return gain;
        }

        public static float BurnFatHour(this Body body, int ticks = 1, float extraActivityFactor = 0f)
        {
            float daily = body.Weight * 10 + body.Height.Value * 6.25f;
            daily *= body.FatBurnRate.Value + extraActivityFactor; // you are moving around
            float perHour = daily / 24f;
            float inFat = perHour / 9000f;
            var burned = inFat * ticks;
            body.Fat.BaseValue -= burned;
            return burned; // Starving
        }

        public static string BodyDesc(this BaseCharacter character, bool isYou = true)
        {
            Body body = character.Body;
            StringBuilder desc = new();
            float muscleRatio = GetMuscleRatio(character.Body);
            desc.Append(MuscleDesc(muscleRatio));
            desc.Append(" muscle and ");

            float fatRatio = GetFatRatio(body);
            desc.Append(FatDesc(fatRatio));
            desc.Append($" fat giving {(isYou ? "you" : character.Gender.HimHer())} a ");

            float fitnessRatio = muscleRatio - fatRatio;
            desc.Append(FitnessDesc(fitnessRatio));
            return $"{desc} look.";
        }

        static string MuscleDesc(float muscleRatio) => muscleRatio switch
        {
            > 0.2f => "More than average",
            < 0 => "Less than average",
            _ => "Average",
        };

        static string FatDesc(float fatRatio) => fatRatio switch
        {
            > 1.1f => "more than average",
            < 0.9f => "less than average",
            _ => "average",
        };

        static string FitnessDesc(float fitnessRatio) => fitnessRatio switch
        {
            > 1.2f => "athletic",
            > 1.1f => "defined",
            < 0.8f => "pudgy",
            < 0.9f => "plump",
            _ => "toned",
        };

        public static float GetFatRatio(this Body body)
        {
            const float avgFat = 25f;
            float fatRatio = body.FatWeight / body.Weight;
            fatRatio = fatRatio * 100 / avgFat;
            return fatRatio;
        }

        public static void GainPercentFat(Body body, float fatGainPercent)
        {
            float currentPercent = body.Fat.BaseValue / body.Weight;
            float goalPercent = currentPercent + Mathf.Clamp(fatGainPercent, 0, 1f);
            while (currentPercent < goalPercent && currentPercent <= 0.7f)
            {
                body.Fat.BaseValue += 0.01f;
                currentPercent = body.Fat.BaseValue / body.Weight;
            }
        }

        public static string HeightDesc(this BaseCharacter character)
        {
            float avgHeight = character.RaceSystem.Race.AverageHeight;
            float ratio = character.Body.Height.Value / avgHeight;
            StringBuilder desc =
                new($"At {character.Body.Height.Value.ConvertCm()} {character.Identity.FirstName} is ");
            desc.Append(HeightRatioDesc(ratio));
            return $"{desc} for a {character.RaceSystem.Race.Title.ToLower()}.";
        }

        static string HeightRatioDesc(float ratio) => ratio switch
        {
            < 0.6f => " significantly shorter than average",
            < 0.8f => " shorter than average",
            > 1.4f => " significantly taller than average",
            > 1.2f => " taller than average",
            _ => " average height",
        };

        public static void ShrinkBody(this Body body, float shrinkBy) => body.Height.BaseValue -= shrinkBy;

        public static void ShrinkBodyByPercent(this Body body, int growBy) =>
            body.Height.BaseValue *= 1f - growBy / 100f;

        public static void GrowBody(this Body body, float growBy) => body.Height.BaseValue += growBy;

        public static void GrowBodyByPercent(this Body body, int growBy) => body.Height.BaseValue *= 1f + growBy / 100f;


        public static float GetMuscleRatio(this Body body)
        {
            const float avgMuscle = 0.28f;
            // 20kg at 160cm = 0%
            // 50kg at 160cm = 100%
            float muscleRatio = body.Muscle.Value / 100f;
            muscleRatio = (muscleRatio - avgMuscle) * 6f;
            return muscleRatio;
        }
    }
}
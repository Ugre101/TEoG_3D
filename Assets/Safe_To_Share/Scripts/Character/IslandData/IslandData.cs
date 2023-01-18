using System;
using Character.BodyStuff;
using Character.EssenceStuff;
using UnityEngine;

namespace Character.IslandData
{
    [Serializable]
    public class IslandData
    {
        public BodyStatData bodyData = new();
        public EssenceData essenceData = new();

        [Serializable]
        public class EssenceData
        {
            public const int IncreaseAmount = 10;
            public int mascValue;
            public int femiValue;
            [SerializeField] MinMaxCurrent masc = new();
            [SerializeField] MinMaxCurrent femi = new();
            public MinMaxCurrent Masc => masc;

            public MinMaxCurrent Femi => femi;

            public void TempLoadFix()
            {
                if (masc.Max < mascValue)
                    masc.Max = mascValue;
                if (femi.Max < femiValue)
                    femi.Max = femiValue;
            }

            MinMaxCurrent GetMinMax(EssenceType type) =>
                type switch
                {
                    EssenceType.Masc => masc,
                    EssenceType.Femi => femi,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

            public int GetValueOfType(EssenceType type) => GetMinMax(type).Current;

            public int GetMaxValueOfType(EssenceType type) => GetMinMax(type).Max;

            public void SetValueOfType(EssenceType type, int value) => GetMinMax(type).Current = value;

            public void IncreaseMaxValueOfType(EssenceType type) => GetMinMax(type).Max += IncreaseAmount;

            public void DecreaseMinValueOfType(EssenceType type) => GetMinMax(type).Min -= IncreaseAmount / 2;
        }

        [Serializable]
        public class BodyStatData
        {
            public int heightValue; // Old way of doing it remove later 
            public int fatValue;
            public int muscleValue;
            public MinMaxCurrent height = new();
            public MinMaxCurrent fat = new();
            public MinMaxCurrent muscle = new();

            public void TempLoadFixer() // TODO Remove in later build
            {
                if (height.Max < heightValue)
                    height.Max = heightValue;
                if (fat.Max < fatValue)
                    fat.Max = fatValue;
                if (muscle.Max < muscleValue)
                    muscle.Max = muscleValue;
            }

            public int GetValueOfType(BodyStatType type) => GetMinMaxOfType(type).Current;
            public int GetMinValueOfType(BodyStatType type) => GetMinMaxOfType(type).Min;
            public int GetMaxValueOfType(BodyStatType type) => GetMinMaxOfType(type).Max;


            MinMaxCurrent GetMinMaxOfType(BodyStatType type) =>
                type switch
                {
                    BodyStatType.Muscle => muscle,
                    BodyStatType.Fat => fat,
                    BodyStatType.Height => height,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                };
            public void SetValueOfType(BodyStatType type, int value) => GetMinMaxOfType(type).Current = value;
            public int IncreaseMaxValueOfType(BodyStatType type) => ++GetMinMaxOfType(type).Max;
            public int DecreaseMinValueOfType(BodyStatType type) => --GetMinMaxOfType(type).Min;
        }

        [Serializable]
        public class MinMaxCurrent
        {
            [SerializeField] int min, max, current;

            public int Current
            {
                get => current;
                set => current = value;
            }

            public int Max
            {
                get => max;
                set => max = value;
            }

            public int Min
            {
                get => min;
                set => min = value;
            }
        }
    }
}
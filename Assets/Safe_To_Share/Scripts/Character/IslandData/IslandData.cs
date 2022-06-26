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
            [SerializeField] MinMaxCurrent masc = new();
            public int femiValue;
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

            public int GetValueOfType(EssenceType type) =>
                type switch
                {
                    EssenceType.Masc => Masc.Current,
                    EssenceType.Femi => Femi.Current,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                };

            public int GetMaxValueOfType(EssenceType type) =>
                type switch
                {
                    EssenceType.Masc => Masc.Max,
                    EssenceType.Femi => Femi.Max,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                };

            public void SetValueOfType(EssenceType type, int value)
            {
                switch (type)
                {
                    case EssenceType.Masc:
                        Masc.Current = value;
                        break;
                    case EssenceType.Femi:
                        Femi.Current = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            public void IncreaseMaxValueOfType(EssenceType type)
            {
                switch (type)
                {
                    case EssenceType.Masc:
                        Masc.Max += IncreaseAmount;
                        break;
                    case EssenceType.Femi:
                        Femi.Max += IncreaseAmount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        [Serializable]
        public class BodyStatData
        {
            public int heightValue; // Max don'y want to rename 
            public MinMaxCurrent height = new();
            public int fatValue;
            public MinMaxCurrent fat = new();
            public int muscleValue;
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

            public int GetValueOfType(BodyStatType type) =>
                type switch
                {
                    BodyStatType.Muscle => muscle.Current,
                    BodyStatType.Fat => fat.Current,
                    BodyStatType.Height => height.Current,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                };

            public int GetMaxValueOfType(BodyStatType type) =>
                type switch
                {
                    BodyStatType.Muscle => muscle.Max,
                    BodyStatType.Fat => fat.Max,
                    BodyStatType.Height => height.Max,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                };

            public void SetValueOfType(BodyStatType type, int value)
            {
                switch (type)
                {
                    case BodyStatType.Muscle:
                        muscle.Current = value;
                        break;
                    case BodyStatType.Fat:
                        fat.Current = value;
                        break;
                    case BodyStatType.Height:
                        height.Current = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            public void IncreaseMaxValueOfType(BodyStatType type)
            {
                switch (type)
                {
                    case BodyStatType.Muscle:
                        muscle.Max++;
                        break;
                    case BodyStatType.Fat:
                        fat.Max++;
                        break;
                    case BodyStatType.Height:
                        height.Max++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
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
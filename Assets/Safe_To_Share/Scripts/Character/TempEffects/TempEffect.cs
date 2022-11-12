using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.TempEffects
{
    [Serializable]
    public class TempEffect
    { 
        [field: SerializeField] public string Source { get; private set; }
        [field: SerializeField] public int HoursLeft { get; private set; }
        [field: SerializeField] public SourceType SourceType { get; private set; }

        public TempEffect(string source, int hoursLeft, SourceType sourceType)
        {
            Source = source;
            HoursLeft = hoursLeft;
            SourceType = sourceType;
        }
        
        public bool TickDown(int ticks)
        {
            HoursLeft -= ticks;
            return HoursLeft <= 0;
        }

        public void AddHours(int toAdd)
        {
            var progressiveLess = toAdd - HoursLeft / 2;
            if (progressiveLess > 0)
                HoursLeft += progressiveLess;
        }
    }

    [Serializable]
    public enum SourceType
    {
        Item,
        Event,
        Sleep,
        Battle,
        Lose,
        Win,
    }
}
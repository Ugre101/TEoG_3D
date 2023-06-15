using System;
using Character;
using Character.PregnancyStuff;
using UnityEngine;

namespace DormAndHome.Dorm {
    [Serializable]
    public class DormMate : BaseCharacter, ITickDay {
        [SerializeField] DormMateSleepIn sleepIn = DormMateSleepIn.Lodge;
        [SerializeField] string workTitle;

        public DormMate(BaseCharacter character) : base(character) { }

        public DormMateSleepIn SleepIn {
            get => sleepIn;
            set => sleepIn = value;
        }

        public string WorkTitle => workTitle;

        public void TickDay(int ticks = 1) => this.TickPregnancy(ticks);

        public void SetWorkTitle(string value) => workTitle = value;
    }

    [Serializable]
    public enum DormMateSleepIn {
        Lodge, Dungeon,
    }
}
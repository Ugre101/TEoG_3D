using System;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character {
    [Serializable]
    public struct BirthDay {
        [SerializeField] int year, day;

        public BirthDay(int year, int day) {
            this.year = year;
            this.day = day;
        }

        public BirthDay(DateSave date) {
            year = date.Year;
            day = date.Day;
        }

        public int Year => year;
        public int Day => day;

        public static BirthDay BirthedToday(int yearsAgo = 0) => new(DateSystem.Year - yearsAgo, DateSystem.Day);
    }
}
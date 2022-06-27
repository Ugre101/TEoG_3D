using System;
using System.Collections.Generic;
using UnityEngine;
//using Character;

namespace Safe_To_Share.Scripts.Static
{
    public static class DateSystem
    {
        public static event Action<int> TickMinute;
        public static event Action<int> NewMinute;
        public static event  Action<int> TickHour;
        public static event Action<int> NewHour; 
        public static event Action<int> TickDay;
        public static event Action<int> NewDay;
        static int day;
        static int hour = 12;
        static int minute;

        static readonly Dictionary<int, string> Months = new()
        {
            {1, "January"}, {2, "February"}, {3, "March"}, {4, "April"}, {5, "May"}, {6, "June"},
            {7, "July"}, {8, "August"}, {9, "September"}, {10, "October"}, {11, "November"}, {12, "December"},
        };

        public static int Year { get; private set; }

        public static int Day
        {
            get => day;
            private set
            {
                if (day < value)
                    TickDay?.Invoke(value - day);
                day = value;
                while (day > 365)
                {
                    day -= 365;
                    Year++;
                }
                NewDay?.Invoke(day);
            }
        }

        public static int Hour
        {
            get => hour;
            private set
            {
                if (hour < value)
                    TickHour?.Invoke(value - hour);
                hour = value;
                // 24
                while (hour >= 24)
                {
                    hour -= 24;
                    Day++;
                }
                NewHour?.Invoke(hour);
            }
        }

        public static int Minute
        {
            get => minute;
            private set
            {
                if (minute < value)
                    TickMinute?.Invoke(value - minute);
                minute = value;
                while (minute >= 60)
                {
                    minute -= 60;
                    Hour++;
                }
                NewMinute?.Invoke(minute);
            }
        }

        public static string CurrentInt => $"{Year} : {GetMonthInt()} : {Day} : {Minute}";

        public static string CurrentShort => $"{Year} : {GetMonth(true)} : {GetDayName(true)} : {Minute}";

        public static string CurrentLong => $"{Year} : {GetMonth()} : {GetDayName()} : {Minute}";

        public static int GetMonthInt()
        {
            if (Day <= 31) return 1;
            if (Day <= 59) return 2;
            if (Day <= 90) return 3;
            if (Day <= 120) return 4;
            if (Day <= 151) return 5;
            if (Day <= 181) return 6;
            if (Day <= 212) return 7;
            if (Day <= 243) return 8;
            if (Day <= 273) return 9;
            if (Day <= 304) return 10;
            if (Day <= 334) return 11;
            return Day <= 365 ? 12 : 0;
        }

        public static string GetMonth(bool shorten = false) =>
            Months.TryGetValue(GetMonthInt(), out string value)
                ? shorten ? value.Substring(0, 3) : value
                : "Err";

        public static string GetDayName(bool shorten = false) =>
            (Day % 7) switch
            {
                0 => shorten ? "Mo" : "Monday",
                1 => shorten ? "Tu" : "Tuesday",
                2 => shorten ? "We" : "Wednesday",
                3 => shorten ? "Th" : "Thursday",
                4 => shorten ? "Fr" : "Friday",
                5 => shorten ? "Sa" : "Saturday",
                6 => shorten ? "Su" : "Sunday",
                _ => shorten ? "Er" : "Error",
            };

        public static void PassMinute(int ticks = 1) => Minute += Mathf.Max(0, ticks);

        public static void PassHour(int ticks = 1)
        {
            TickMinute?.Invoke(ticks * 60);
            Hour += ticks;
        }

        public static DateSave Save() => new(Minute, Hour, Day, Year);

        public static void Load(DateSave save)
        {
            minute = save.Min;
            hour = save.Hour;
            day = save.Day;
            Year = save.Year;
            NewMinute?.Invoke(Minute);
            NewHour?.Invoke(Hour);
            NewDay?.Invoke(Day);
        }

        public static int DateSaveYearsAgo(DateSave date) => Year - date.Year;

        public static int DateSaveDaysAgo(DateSave date)
        {
            int days = DateSaveYearsAgo(date) * 365;
            days += Day - date.Day;
            return days;
        }
        public static int DateSaveHoursAgo(DateSave date)
        {
            int hours = DateSaveDaysAgo(date) * 24;
            hours += Hour - date.Hour;
            return hours;
        }

        public static int DateSaveMinutesAgo(DateSave date)
        {
            int minutes = DateSaveHoursAgo(date) * 60;
            minutes += Minute - date.Min;
            return minutes;
        }
        
       // public static BirthDay BirthedToday(int yearsAgo = 0) => new BirthDay(Year - yearsAgo, Day);
    }

    [Serializable]
    public struct DateSave
    {
        [SerializeField] int min, year, day, hour;

        public DateSave(int min, int hour, int day, int year)
        {
            this.min = min;
            this.hour = hour;
            this.day = day;
            this.year = year;
        }

        public int Year => year;

        public int Day => day;

        public int Hour => hour;

        public int Min => min;
    }
}
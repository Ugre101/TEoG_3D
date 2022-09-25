using System;
using System.Collections.Generic;
using System.Linq;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.Family
{
    public static class DayCare
    {
        static List<Child> Children { get; set; } = new();

        public static IReadOnlyDictionary<int, Child> ChildDict { get; private set; } = new Dictionary<int, Child>();

        public static void AddChild(Child newChild)
        {
            Children.Add(newChild);
            ChildDict = Children.ToDictionary(c => c.Identity.ID);
        }

        public static DayCareSave Save() => new(Children);

        public static void Load(DayCareSave toLoad)
        {
            Children = toLoad.Children;
            ChildDict = Children.ToDictionary(c => c.Identity.ID);
        }

        public static void RenameChild(Child child,string firstName)
        {
            for (var index = 0; index < Children.Count; index++)
            {
                if (Children[index].Identity.ID == child.Identity.ID)
                {
                    Children[index].Identity.ChangeFirstName(firstName);
                    break;
                }
            }
        }

        public static void TickBirthDays(int ticks = 1)
        {
            List<Child> birthDayChilds = Children.Where(child => child.MyBirthDayToday()).ToList();
            foreach (Child birthDayChild in birthDayChilds)
                EventLog.AddEvent(
                    $"{birthDayChild.Identity.FullName} is celebrating their {DateSystem.Year - birthDayChild.Identity.BirthDay.Year} birthday.");
        }
    }

    [Serializable]
    public struct DayCareSave
    {
        [SerializeField] List<Child> children;

        public DayCareSave(List<Child> children) => this.children = children;

        public List<Child> Children => children;
    }
}
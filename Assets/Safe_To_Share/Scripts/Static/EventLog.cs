using System;
using System.Collections.Generic;

namespace Safe_To_Share.Scripts.Static
{
    public static class EventLog
    {
        public static event Action<string> NewEvent;
        public static readonly List<string> Events = new List<string>();

        public static void AddEvent(string eventText)
        {
            Events.Add(eventText);
            NewEvent?.Invoke(eventText);
        }
    }
}
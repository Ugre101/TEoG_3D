using System.Collections.Generic;
using System.Text;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public class GameUiEventLog : MonoBehaviour
    {
        const float CheckInterval = 0.5f;
        [SerializeField] TextMeshProUGUI eventLog;
        [SerializeField] float displayEventTime = 10f;
        readonly List<TimedEventLog> eventLogs = new();
        float lastCheck;

        void Start() => PrintEventLog();

        void Update()
        {
            if (!EnoughTimeAsPassed())
                return;
            if (eventLogs.RemoveAll(l => l.AddedTime + displayEventTime <= Time.time) > 0)
                PrintEventLog();
            lastCheck = Time.time;
        }

        void OnEnable() => EventLog.NewEvent += AddText;

        void OnDisable() => EventLog.NewEvent -= AddText;

        bool EnoughTimeAsPassed() => lastCheck + CheckInterval <= Time.time;

        void AddText(string text)
        {
            eventLogs.Add(new TimedEventLog(text, Time.time));
            string insertText = $"{text}\n\n";
            eventLog.text += insertText;
        }

        void PrintEventLog()
        {
            switch (eventLogs.Count)
            {
                case 0:
                    eventLog.text = string.Empty;
                    return;
                case 1:
                    eventLog.text = eventLogs[0].Text;
                    return;
            }

            StringBuilder sb = new();
            foreach (TimedEventLog log in eventLogs)
            {
                sb.AppendLine(log.Text);
                sb.AppendLine();
            }

            eventLog.text = sb.ToString();
        }

        readonly struct TimedEventLog
        {
            public readonly string Text;
            public readonly float AddedTime;

            public TimedEventLog(string text, float addedTime)
            {
                Text = text;
                AddedTime = addedTime;
            }
        }
    }
}
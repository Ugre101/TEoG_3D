
using System.Text;
using Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus
{
    public class BigEventLog : GameMenu
    {
        [SerializeField] TextMeshProUGUI eventLog;


        void OnEnable()
        {
            PrintEventLog();
            EventLog.NewEvent += AddedText;
        }

        void OnDisable() => EventLog.NewEvent -= AddedText;

        void PrintEventLog()
        {
            if (EventLog.Events.Count == 0)
            {
                eventLog.text = string.Empty;
                return;
            }
            StringBuilder sb = new();
            for (int i = EventLog.Events.Count; i-- > 1;)
            {
                sb.AppendLine(EventLog.Events[i]);
                sb.AppendLine();
            }
            sb.AppendLine(EventLog.Events[0]);

            eventLog.text = sb.ToString();
        }

        void AddedText(string text)
        {
            string insertText = text;
            if (eventLog.text.Length > 0)
                insertText += "\n\n";
            eventLog.text = eventLog.text.Insert(0, $"{insertText}");
        }
    }
}
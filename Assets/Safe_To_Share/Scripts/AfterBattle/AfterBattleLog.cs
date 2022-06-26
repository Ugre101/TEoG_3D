using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleLog : MonoBehaviour
    {
        [SerializeField] Button clearBtn;
        [SerializeField] TextMeshProUGUI textLog;

        readonly List<string> log = new();
        readonly WaitForSeconds waitForSeconds = new(10f);
        readonly WaitForSeconds waitForSecondsShort = new(6f);


        void Start()
        {
            clearBtn.onClick.AddListener(Clear);
            clearBtn.gameObject.SetActive(false);
        }

        public void AddNewText(string text)
        {
            if (text.Length <= 0)
                return;
            log.Add(text);
            PrintLog();
            StartCoroutine(DelayedErasure(text));
            clearBtn.gameObject.SetActive(true);
        }

        void PrintLog()
        {
            if (log.Count <= 0)
            {
                clearBtn.gameObject.SetActive(false);
                Clear();
            }
            else
            {
                StringBuilder sb = new();
                foreach (string s in log)
                    sb.AppendLine(s);

                textLog.text = sb.ToString();
            }
        }

        IEnumerator DelayedErasure(string length)
        {
            yield return length.Length < 30 ? waitForSecondsShort : waitForSeconds;
            log.RemoveAt(log.Count - 1);
            PrintLog();
        }


        void Clear() => textLog.text = string.Empty;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class FirstStartHelper : MonoBehaviour
    {
        public static bool ShowHelp;
        [SerializeField] List<HelpText> helpTexts = new();
        [SerializeField, Range(0.5f, 5f),] float showFirstAfter;
        [SerializeField, Range(0f, 10f),] float showTime, fadeTime, inBetweenTime;
        [SerializeField, HideInInspector,] string guid;
        WaitForSeconds showForSeconds;
        WaitForSeconds waitBetweenForSeconds;
        WaitForSeconds waitForSecondsBeforeFirst;

        static HashSet<string> Seen { get; } = new();

        void Start()
        {
#if UNITY_EDITOR
            ShowHelp = true;
#endif
            if (ShowHelp && Seen.Add(guid))
            {
                waitForSecondsBeforeFirst = new WaitForSeconds(showFirstAfter);
                showForSeconds = new WaitForSeconds(showTime);
                waitBetweenForSeconds = new WaitForSeconds(inBetweenTime);
                StartCoroutine(ShowMessages());
            }
            else
                RemoveHelpText();
        }

# if UNITY_EDITOR
        void OnValidate()
        {
            if (string.IsNullOrEmpty(guid))
                guid = Guid.NewGuid().ToString();
        }
#endif

        void RemoveHelpText() => Destroy(gameObject);

        IEnumerator ShowMessages()
        {
            yield return waitForSecondsBeforeFirst;
            foreach (var message in helpTexts)
            {
                message.onText.text = message.message;
                yield return showForSeconds;
                float timeFaded = 0;
                Color orgColor = message.onText.color;
                while (fadeTime > timeFaded)
                {
                    timeFaded += Time.deltaTime;
                    var color = message.onText.color;
                    color.a = 1f * (1 - timeFaded / fadeTime);

                    message.onText.color = color;
                    yield return null;
                }

                message.onText.text = string.Empty;
                message.onText.color = orgColor;
                yield return waitBetweenForSeconds;
            }

            RemoveHelpText();
        }

        [Serializable]
        struct HelpText
        {
            [TextArea] public string message;

            public TextMeshProUGUI onText;
        }
    }
}
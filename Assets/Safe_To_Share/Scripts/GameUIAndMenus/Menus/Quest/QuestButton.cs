using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestStuff.UI
{
    public class QuestButton : MonoBehaviour
    {
        public static event Action<QuestInfo, QuestProgress> ShowMeOnLargePage;
        [SerializeField] Button btn;
        [SerializeField] TextMeshProUGUI title, desc, world,progress;
        public void Setup(QuestInfo info, QuestProgress questProgress)
        {

            btn.onClick.AddListener(ShowOnLarge);
            title.text = info.Title;
            desc.text = info.Desc;
            world.text = info.World;
            int i = questProgress.Progress;
            progress.text = i > 0 ? string.Empty : $"Progress: {{{i}}}";

            void ShowOnLarge() => ShowMeOnLargePage?.Invoke(info, questProgress);
        }

    }
}
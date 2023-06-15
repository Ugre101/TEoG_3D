using System;
using QuestStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Quest {
    public sealed class QuestButton : MonoBehaviour {
        [SerializeField] Button btn;
        [SerializeField] TextMeshProUGUI title, desc, world, progress;
        public static event Action<QuestInfo, QuestProgress> ShowMeOnLargePage;

        public void Setup(QuestInfo info, QuestProgress questProgress) {
            btn.onClick.AddListener(ShowOnLarge);
            title.text = info.Title;
            desc.text = info.Desc;
            world.text = info.World;
            var i = questProgress.Progress;
            progress.text = i > 0 ? string.Empty : $"Progress: {{{i}}}";

            void ShowOnLarge() => ShowMeOnLargePage?.Invoke(info, questProgress);
        }
    }
}
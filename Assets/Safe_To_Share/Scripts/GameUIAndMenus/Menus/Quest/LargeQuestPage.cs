using QuestStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Quest
{
    public sealed class LargeQuestPage : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI defaultText;
        [SerializeField] TextMeshProUGUI title, desc, world, progress;

        void Start()
        {
            QuestButton.ShowMeOnLargePage += ShowQuest;
            SetText(string.Empty, string.Empty, string.Empty, string.Empty);
            defaultText.text = "Click on a Quest";
        }

        void ShowQuest(QuestInfo arg1, QuestProgress arg2)
        {
        }

        void SetText(string titleText, string descText, string worldText, string progressText)
        {
            defaultText.text = string.Empty;
            title.text = titleText;
            desc.text = descText;
            world.text = worldText;
            progress.text = progressText;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using QuestStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Quest
{
    public class QuestMenu : GameMenu
    {
        [SerializeField] QuestButton questButton;

        [SerializeField] Transform activeQuests;

        void OnEnable() => Setup();

        void Setup()
        {
            foreach (Transform child in activeQuests)
                Destroy(child.gameObject);
            PrintQuests();
        }

        void PrintQuests()
        {
            if (!PlayerQuests.PrintQuests.Any())
                return;
            foreach (KeyValuePair<QuestInfo, QuestProgress> pair in PlayerQuests.PrintQuests)
                Instantiate(questButton, activeQuests).Setup(pair.Key, pair.Value);
        }
    }
}
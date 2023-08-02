using System;
using Dialogue;
using QuestStuff;
using SceneStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DialogueAndEventMenu {
    public sealed class DialogueMenu : DialogueAndEventShared {
        [SerializeField] GameObject normalDial, imageDial;
        [SerializeField] Image image;
        public void Setup(BaseDialogue dialogue) {
            CurrentDialogue = dialogue;
            CurrentNode = dialogue.GetRootNode();
            HandleNode(CurrentNode);
        }

        void HandleNode(DialogueBaseNode node) {
            AddOptionButtons(node);
            ShowNodeText(node);
            HandleImage(node);
            HandleActions();
        }

        protected override void HandleOption(DialogueBaseNode obj) {
            CurrentNode = obj;
            HandleNode(obj);
            switch (CurrentNode) {
                case DialogueQuestNode dialogueQuestNode:
                    PlayerQuests.AddQuest(dialogueQuestNode.Quest);
                    break;
                case OpenShopDialogueNode openShopNode:
                    gameCanvas.OpenShopMenu(openShopNode.ShopTitle, openShopNode.ShopItems.SellingItems);
                    break;
                case ServiceMenuDialogueNode openServiceMenuNode:
                    gameCanvas.OpenServiceMenu(openServiceMenuNode.ShopTitle, openServiceMenuNode.Services);
                    break;
                case CloseDialogue close:
                    gameCanvas.CloseMenus();
                    break;
                case PreBattleDialogue preBattleDialogue:
                    SceneLoader.Instance.LoadCombatIfNotAlready();
                    SceneLoader.Instance.LoadCombatUIIfNotAlready();
                    break;
                case { } baseNode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(CurrentNode));
            }
        }

        void HandleActions() {
            foreach (var dialogueBaseAction in CurrentNode.Actions)
                dialogueBaseAction.Invoke(Player);
        }

        void HandleImage(DialogueBaseNode obj) {
            var hasImage = obj.Image != null;
            imageDial.SetActive(hasImage);
            normalDial.SetActive(hasImage is false);
            if (hasImage)
                image.sprite = obj.Image;
        }
    }
}
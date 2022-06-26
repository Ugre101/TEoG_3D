using System;
using System.Linq;
using System.Text;
using Character.PlayerStuff;
using Character.VoreStuff;
using Dialogue;
using Dialogue.DialogueActions;
using Dialogue.Events;
using Dialogue.UI;
using QuestStuff;
using Safe_to_Share.Scripts.CustomClasses;
using SceneStuff;
using UnityEngine;

namespace GameUIAndMenus.DialogueAndEventMenu
{
    public class DialogueMenu : DialogueAndEventShared
    {
        public void Setup(BaseDialogue dialogue)
        {
            currentDialogue = dialogue;
            currentNode = dialogue.GetRootNode() as DialogueBaseNode;
            AddOptionButtons(currentNode);
            ShowNodeText(currentNode);
        }
        protected override void HandleOption(DialogueBaseNode obj)
        {
            currentNode = obj;
            foreach (DialogueBaseAction dialogueBaseAction in currentNode.Actions) 
                dialogueBaseAction.Invoke(Player);
            AddOptionButtons(currentNode);
            ShowNodeText(currentNode);
            switch (currentNode)
            {
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
                    throw new ArgumentOutOfRangeException(nameof(currentNode));
            }
        }
    }
}
using System;
using Dialogue;
using Dialogue.DialogueActions;
using GameUIAndMenus.DialogueAndEventMenu;
using QuestStuff;
using SceneStuff;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DialogueAndEventMenu
{
    public class DialogueMenu : DialogueAndEventShared
    {
        public void Setup(BaseDialogue dialogue)
        
        {
            CurrentDialogue = dialogue;
            CurrentNode = dialogue.GetRootNode() as DialogueBaseNode;
            AddOptionButtons(CurrentNode);
            ShowNodeText(CurrentNode);
        }

        protected override void HandleOption(DialogueBaseNode obj)
        {
            CurrentNode = obj;
            foreach (DialogueBaseAction dialogueBaseAction in CurrentNode.Actions)
                dialogueBaseAction.Invoke(Player);
            AddOptionButtons(CurrentNode);
            ShowNodeText(CurrentNode);
            switch (CurrentNode)
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
                    throw new ArgumentOutOfRangeException(nameof(CurrentNode));
            }
        }
    }
}
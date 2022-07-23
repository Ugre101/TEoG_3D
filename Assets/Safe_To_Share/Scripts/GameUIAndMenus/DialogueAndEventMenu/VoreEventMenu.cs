using System;
using Character;
using Character.VoreStuff;
using Dialogue;
using Dialogue.DialogueActions;
using QuestStuff;
using SceneStuff;

namespace GameUIAndMenus.DialogueAndEventMenu
{
    public class VoreEventMenu : DialogueAndEventShared
    {
        VoreOrgan organ;
        BaseCharacter pred;
        Prey prey;

        public void Setup(BaseDialogue dialogue, BaseCharacter pred, Prey prey, VoreOrgan organ)
        {
            currentDialogue = dialogue;
            this.pred = pred;
            this.prey = prey;
            this.organ = organ;
            currentNode = dialogue.GetRootNode() as DialogueBaseNode;
            AddOptionButtons(currentNode);
            ShowNodeText(currentNode);
        }

        protected override void HandleOption(DialogueBaseNode obj)
        {
            currentNode = obj;
            foreach (DialogueBaseAction dialogueBaseAction in currentNode.Actions)
                dialogueBaseAction.Invoke(pred);
            foreach (DialogueVoreAction currentNodeVoreAction in obj.VoreActions)
                currentNodeVoreAction.Invoke(pred, prey, organ);
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
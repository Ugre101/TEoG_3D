using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue.DialogueActions;
using Dialogue.DialogueActions.Vore;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Dialogue
{
    public class DialogueBaseNode : BaseEditorCanvasNode
    {
        [SerializeField] string playerText;
        [SerializeField] string title;
        [SerializeField, TextArea,] string[] text;
        [SerializeField] bool canEscapeOut;
        [SerializeReference] List<DialogueBaseAction> actions = new();
        [SerializeReference] List<DialogueVoreAction> voreActions = new();

        public string Title => title;
        public string[] Text => text;
        public string PlayerText => playerText;
        public virtual bool ShowNode => true;

        public bool CanEscapeOut => canEscapeOut;

        public List<DialogueVoreAction> VoreActions => voreActions;

        public List<DialogueBaseAction> Actions => actions;

        public bool MeetsActionsConditions() => Actions.All(dialogueBaseAction => dialogueBaseAction.MeetsCondition());
#if UNITY_EDITOR
        public void AddAction(int i)
        {
            switch (i)
            {
                case 0:
                    Actions.Add(new AddToDorm());
                    break;
                case 1:
                    VoreActions.Add(new ReleasePrey());
                    break;
                case 2:
                    VoreActions.Add(new AddVoreTempMod());
                    break;
            }
        }
#endif
    }
}
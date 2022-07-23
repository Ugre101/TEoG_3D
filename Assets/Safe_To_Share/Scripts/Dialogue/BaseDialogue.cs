using System;
using System.Linq;
using Character.PlayerStuff;
using Character.VoreStuff;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/new Dialogue", order = 0)]
    public class BaseDialogue : BaseEditorCanvasObject
    {
        public override void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
                nodes.Add(MakeNode<DialogueBaseNode>());
            if (AssetDatabase.GetAssetPath(this) != string.Empty)
                foreach (BaseEditorCanvasNode dialogueBaseNode in GetAllNodes().Where(dialogueBaseNode =>
                             AssetDatabase.GetAssetPath(dialogueBaseNode) == string.Empty))
                    AssetDatabase.AddObjectToAsset(dialogueBaseNode, this);
#endif
        }

        public static event Action<BaseDialogue> StartDialogue;
        public static event Action<BaseDialogue, Player, Prey, VoreOrgan> StartVoreEvent;
        public void StartTalking() => StartDialogue?.Invoke(this);

        public void TriggerVoreEvent(Player player, Prey prey, VoreOrgan voreOrgan) =>
            StartVoreEvent?.Invoke(this, player, prey, voreOrgan);


#if UNITY_EDITOR

        public void CreateNewNode<TNodeType>(BaseEditorCanvasNode parent) where TNodeType : DialogueBaseNode
        {
            TNodeType newNode = MakeNode<TNodeType>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created new dialogue node");
            if (parent != null)
            {
                parent.ChildNodeIds.Add(newNode.name);
                Rect offsetRect = parent.rect;
                offsetRect.x += parent.rect.width;
                newNode.rect = offsetRect;
            }

            Undo.RecordObject(this, "Added Dialogue Node");
            nodes.Add(newNode);
            IfPreBattleNodeAddThis(newNode);
            nodeChildDict = null;
        }

        void IfPreBattleNodeAddThis(DialogueBaseNode newNode)
        {
            if (newNode is not PreBattleDialogue preBattleDialogue)
                return;
            StartBattleDialogue startBattleDialogue = MakeNode<StartBattleDialogue>();
            Undo.RegisterCreatedObjectUndo(startBattleDialogue, "Added start battle option");
            preBattleDialogue.ChildNodeIds.Add(startBattleDialogue.name);
            Rect extraOffset = preBattleDialogue.rect;
            extraOffset.height *= 0.9f;
            extraOffset.x += preBattleDialogue.rect.width;
            startBattleDialogue.rect = extraOffset;
            nodes.Add(startBattleDialogue);
        }

#endif
    }
}
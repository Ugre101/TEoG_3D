using System;
using Character.PlayerStuff;
using Character.VoreStuff;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEditor;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/new Dialogue", order = 0)]
    public sealed class BaseDialogue : BaseEditorCanvasObject<DialogueBaseNode> {
        public static event Action<BaseDialogue> StartDialogue;
        public static event Action<BaseDialogue, Player, Prey, VoreOrgan> StartVoreEvent;
        public void StartTalking() => StartDialogue?.Invoke(this);

        public void TriggerVoreEvent(Player player, Prey prey, VoreOrgan voreOrgan) =>
            StartVoreEvent?.Invoke(this, player, prey, voreOrgan);

        public override void OnBeforeSerialize() {
#if UNITY_EDITOR

            base.OnBeforeSerialize();
            // foreach (var dialogueBaseNode in nodes) {
            //     Debug.Log(dialogueBaseNode.Actions.Count);
            //     foreach (var dialogueBaseAction in dialogueBaseNode.Actions) {
            //         if (AssetDatabase.GetAssetPath(dialogueBaseAction) == string.Empty) {
            //             AssetDatabase.AddObjectToAsset(dialogueBaseAction,this);
            //         }
            //     }
            // }
#endif
        }

#if UNITY_EDITOR

        public override TNode CreateChildNode<TNode>(BaseEditorCanvasNode parentNode) {
            var newNode = base.CreateChildNode<TNode>(parentNode);
            IfPreBattleNodeAddThis(newNode);
            return newNode;
        }


        void IfPreBattleNodeAddThis(DialogueBaseNode newNode) {
            if (newNode is not PreBattleDialogue preBattleDialogue)
                return;
            var startBattleDialogue = MakeNode<StartBattleDialogue>();
            Undo.RegisterCreatedObjectUndo(startBattleDialogue, "Added start battle option");
            preBattleDialogue.ChildNodeIds.Add(startBattleDialogue.name);
            var extraOffset = preBattleDialogue.rect;
            extraOffset.height *= 0.9f;
            extraOffset.x += preBattleDialogue.rect.width;
            startBattleDialogue.rect = extraOffset;
            nodes.Add(startBattleDialogue);
        }

#endif
    }
}
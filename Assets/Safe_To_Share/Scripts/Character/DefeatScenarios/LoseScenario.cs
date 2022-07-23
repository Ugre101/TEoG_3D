using System.Linq;
using Character.DefeatScenarios.Nodes;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Defeated
{
    [CreateAssetMenu(fileName = "New Lose Scenario", menuName = "Defeat/Lose Scenario", order = 0)]
    public class LoseScenario : BaseEditorCanvasObject
    {
        [SerializeField] string title;

        public override void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
                nodes.Add(MakeNode<LoseScenarioNode>());
            if (AssetDatabase.GetAssetPath(this) != string.Empty)
                foreach (var node in nodes.Where(n =>
                             AssetDatabase.GetAssetPath(n) == string.Empty))
                    AssetDatabase.AddObjectToAsset(node, this);

#endif
        }


#if UNITY_EDITOR

        public void CreateChildNode<TNode>(BaseEditorCanvasNode parentNode) where TNode : LoseScenarioNode
        {
            TNode newNode = MakeNode<TNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created lose scenario node");
            if (parentNode != null)
            {
                parentNode.ChildNodeIds.Add(newNode.name);
                Rect offsetRect = parentNode.rect;
                offsetRect.x += parentNode.rect.width;
                newNode.rect = offsetRect;
            }

            Undo.RecordObject(this, "Added scenario Node");
            nodes.Add(newNode);
            nodeChildDict = null;
        }
#endif
    }
}
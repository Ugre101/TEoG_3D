using System.Collections.Generic;
using System.Linq;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace CustomClasses
{
    public abstract class BaseEditorCanvasObject : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector,] string guid;
        [SerializeField] protected List<BaseEditorCanvasNode> nodes = new();
        protected Dictionary<string, BaseEditorCanvasNode> nodeChildDict;

        public string Guid => guid;

        Dictionary<string, BaseEditorCanvasNode> NodeChildDict =>
            nodeChildDict ??= GetAllNodes().ToDictionary(n => n.name);
#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif

        public abstract void OnBeforeSerialize();

        public void OnAfterDeserialize()
        {
        }

        public IEnumerable<BaseEditorCanvasNode> GetChildNodes(BaseEditorCanvasNode parentNode)
        {
            foreach (string childID in parentNode.ChildNodeIds)
                if (NodeChildDict.TryGetValue(childID, out BaseEditorCanvasNode childNode))
                    yield return childNode;
        }

        public IEnumerable<BaseEditorCanvasNode> GetAllNodes() => nodes;
        public BaseEditorCanvasNode GetRootNode() => nodes[0];
#if UNITY_EDITOR
        protected static TNodeType MakeNode<TNodeType>() where TNodeType : BaseEditorCanvasNode
        {
            TNodeType newNode = CreateInstance<TNodeType>();
            newNode.name = System.Guid.NewGuid().ToString();
            return newNode;
        }

        public BaseEditorCanvasNode GetNodeAtPoint(Vector2 pos) =>
            GetAllNodes().LastOrDefault(dialogueBaseNode => dialogueBaseNode.rect.Contains(pos));

        public void DeleteChildNode(BaseEditorCanvasNode node)
        {
            Undo.RecordObject(this, "Deleting a node");
            nodes.Remove(node);
            nodeChildDict = null;
            foreach (BaseEditorCanvasNode baseNode in GetAllNodes())
                baseNode.ChildNodeIds.Remove(node.name);
            Undo.DestroyObjectImmediate(node);
        }
#endif
    }
}
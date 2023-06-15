using System.Collections.Generic;
using System.Linq;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace CustomClasses {
    public abstract class BaseEditorCanvasObject<N> : ScriptableObject, ISerializationCallbackReceiver
        where N : BaseEditorCanvasNode {
        [SerializeField, HideInInspector,] string guid;
        [SerializeField] protected List<N> nodes = new();
        protected Dictionary<string, N> nodeChildDict;

        public string Guid => guid;

        Dictionary<string, N> NodeChildDict => nodeChildDict ??= GetAllNodes().ToDictionary(n => n.name);
#if UNITY_EDITOR
        public virtual void OnValidate() {
            var path = AssetDatabase.GetAssetPath(this);
            guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif

        public virtual void OnBeforeSerialize() {
#if UNITY_EDITOR

            if (nodes.Count == 0)
                nodes.Add(MakeNode<N>());
            if (AssetDatabase.GetAssetPath(this) == string.Empty)
                return;
            foreach (var node in nodes.Where(n => AssetDatabase.GetAssetPath(n) == string.Empty))
                AssetDatabase.AddObjectToAsset(node, this);
#endif
        }

        public void OnAfterDeserialize() { }

        public IEnumerable<N> GetChildNodes(N parentNode) {
            foreach (var childID in parentNode.ChildNodeIds)
                if (NodeChildDict.TryGetValue(childID, out var childNode))
                    yield return childNode;
        }

        public IEnumerable<N> GetAllNodes() => nodes;
        public N GetRootNode() => nodes[0];
#if UNITY_EDITOR
        public virtual TNode CreateChildNode<TNode>(BaseEditorCanvasNode parentNode) where TNode : N {
            var newNode = MakeNode<TNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created node");
            if (parentNode != null) {
                parentNode.ChildNodeIds.Add(newNode.name);
                var offsetRect = parentNode.rect;
                offsetRect.x += parentNode.rect.width;
                newNode.rect = offsetRect;
            }

            Undo.RecordObject(this, "Added Node");
            nodes.Add(newNode);
            nodeChildDict = null;
            return newNode;
        }

        protected static TNodeType MakeNode<TNodeType>() where TNodeType : N {
            var newNode = CreateInstance<TNodeType>();
            newNode.name = System.Guid.NewGuid().ToString();
            return newNode;
        }

        public N GetNodeAtPoint(Vector2 pos) =>
            GetAllNodes().LastOrDefault(dialogueBaseNode => dialogueBaseNode.rect.Contains(pos));

        public void DeleteChildNode(N node) {
            Undo.RecordObject(this, "Deleting a node");
            nodes.Remove(node);
            nodeChildDict = null;
            foreach (var baseNode in GetAllNodes())
                baseNode.ChildNodeIds.Remove(node.name);
            Undo.DestroyObjectImmediate(node);
        }
#endif
    }
}
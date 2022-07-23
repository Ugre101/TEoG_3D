using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Safe_to_Share.Scripts.CustomClasses
{
    public class BaseEditorCanvasNode : ScriptableObject
    {
        [SerializeField] List<string> childNodeIds = new();
        public virtual List<string> ChildNodeIds => childNodeIds;
#if UNITY_EDITOR
        [HideInInspector] public Rect rect = new(Vector2.zero, new Vector2(200, 150));

        public void AddChild(BaseEditorCanvasNode childNode)
        {
            Undo.RecordObject(this, "Linked child node");
            ChildNodeIds.Add(childNode.name);
        }

        public void RemoveChild(BaseEditorCanvasNode toRemove)
        {
            Undo.RecordObject(this, "Unlinked child node");
            ChildNodeIds.Remove(toRemove.name);
        }
#endif
    }
}
using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEditor;
using UnityEngine;

namespace CustomClasses.Editor
{
    public abstract class BaseNodeBuilderWindows : EditorWindow
    {
        protected const int CanvasSize = 4000;
        [NonSerialized] protected BaseEditorCanvasNode creatingNode;
        [NonSerialized] protected BaseEditorCanvasNode deletingNode;
        [NonSerialized] protected BaseEditorCanvasNode draggedNode;
        [NonSerialized] protected bool DraggingCanvas;
        [NonSerialized] protected Vector2 DraggingCanvasOffset;
        [NonSerialized] protected Vector2 DraggingOffset;
        [NonSerialized] protected BaseEditorCanvasNode linkingNode;
        protected GUIStyle NodeStyle;
        protected Vector2 ScrollPos;

        protected virtual void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
            NodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node0") as Texture2D,
                    textColor = Color.white,
                },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12),
            };
        }

        protected abstract void SelectionChanged();

        protected static void DrawNodeConnection(BaseEditorCanvasObject canvasObject, BaseEditorCanvasNode node)
        {
            foreach (BaseEditorCanvasNode childNode in canvasObject.GetChildNodes(node))
            {
                Vector3 startPos = new Vector2(node.rect.xMax, node.rect.center.y);
                Vector3 endPos = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 offset = endPos - startPos;
                offset.y = 0;
                offset.x *= 0.8f;
                Handles.DrawBezier(startPos, endPos, startPos + offset, endPos - offset, Color.white, null, 4f);
            }
        }

        protected void PrintButtons(BaseEditorCanvasNode node)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
                creatingNode = node;

            if (linkingNode == null)
            {
                if (GUILayout.Button("Link"))
                    linkingNode = node;
            }
            else if (linkingNode == node)
            {
                if (GUILayout.Button("Cancel"))
                    linkingNode = null;
            }
            else if (linkingNode.ChildNodeIds.Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    linkingNode.RemoveChild(node);
                    linkingNode = null;
                }
            }
            else if (GUILayout.Button("To"))
            {
                linkingNode.AddChild(node);
                linkingNode = null;
            }

            if (GUILayout.Button("Delete"))
                deletingNode = node;
            GUILayout.EndHorizontal();
        }
    }
}
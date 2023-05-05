using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEditor;
using UnityEngine;

namespace CustomClasses.Editor
{
    public abstract class BaseNodeBuilderWindows<T,N> : EditorWindow where T : BaseEditorCanvasObject<N> where N : BaseEditorCanvasNode
    {
        protected const int CanvasSize = 4000;
        [NonSerialized] protected N creatingNode;
        [NonSerialized] protected N deletingNode;
        [NonSerialized] protected N draggedNode;
        [NonSerialized] protected bool DraggingCanvas;
        [NonSerialized] protected Vector2 DraggingCanvasOffset;
        [NonSerialized] protected Vector2 DraggingOffset;
        [NonSerialized] protected N linkingNode;
        protected GUIStyle NodeStyle;
        protected Vector2 ScrollPos;

        protected T Selected;
        
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

        protected void SelectionChanged()
        {
            var newObjet = Selection.activeObject;
            if (newObjet is T match)
            {
                Selected = match;
                Repaint();
            }
        }

        protected static void DrawNodeConnection(BaseEditorCanvasObject<N> canvasObject, N node)
        {
            foreach (N childNode in canvasObject.GetChildNodes(node))
            {
                Vector3 startPos = new Vector2(node.rect.xMax, node.rect.center.y);
                Vector3 endPos = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 offset = endPos - startPos;
                offset.y = 0;
                offset.x *= 0.8f;
                Handles.DrawBezier(startPos, endPos, startPos + offset, endPos - offset, Color.white, null, 4f);
            }
        }

        protected void PrintButtons(N node)
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

        protected void ProcessEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown when draggedNode == null:
                {
                    draggedNode = Selected.GetNodeAtPoint(Event.current.mousePosition + ScrollPos);
                    if (draggedNode != null)
                    {
                        DraggingOffset = draggedNode.rect.position - Event.current.mousePosition;
                        Selection.activeObject = draggedNode;
                    }
                    else
                    {
                        DraggingCanvas = true;
                        DraggingCanvasOffset = Event.current.mousePosition + ScrollPos;
                        Selection.activeObject = Selected;
                    }

                    break;
                }
                case EventType.MouseDrag when draggedNode != null:
                    Undo.RecordObject(Selected, "Move node");
                    draggedNode.rect.position = Event.current.mousePosition + DraggingOffset;
                    GUI.changed = true;
                    break;
                case EventType.MouseDrag when DraggingCanvas:
                    ScrollPos = DraggingCanvasOffset - Event.current.mousePosition;
                    GUI.changed = true;
                    break;
                case EventType.MouseUp when draggedNode != null:
                    draggedNode = null;
                    break;
                case EventType.MouseUp when DraggingCanvas:
                    DraggingCanvas = false;
                    break;
            }
        }
    }
}
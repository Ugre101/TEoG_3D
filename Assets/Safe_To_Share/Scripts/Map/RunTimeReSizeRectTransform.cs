using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
    public class RunTimeReSizeRectTransform : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action ChangedSize;
        static Vector2 newSize;
        [SerializeField] float minSize = 100f, maxSize = 600f;
        [SerializeField] bool invertX, invertY;
        [SerializeField] RectTransform rect;

        Vector2 lastPos;

        Vector2 startPos;

        void Start()
        {
            if (newSize != Vector2.zero)
                rect.sizeDelta = newSize;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = eventData.position;
            lastPos = startPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            ChangeAccordingToMousePos(eventData.position, lastPos);
            lastPos = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData) => newSize = rect.sizeDelta;

        void ChangeAccordingToMousePos(Vector2 mousePos, Vector2 prevPos)
        {
            Vector2 diff = prevPos - mousePos;
            float x = invertX ? -diff.x : diff.x;
            float y = invertY ? -diff.y : diff.y;
            bool didXChangeMost = Mathf.Abs(y) < Mathf.Abs(x);
            float mostChanged = didXChangeMost ? x : y;
            ChangeRectSize(mostChanged);
        }

        void ChangeRectSize(float changeValue)
        {
            float value = Mathf.Clamp(rect.sizeDelta.x + changeValue, minSize, maxSize);
            Vector2 temp = new(value, value);
            rect.sizeDelta = temp;
            ChangedSize?.Invoke();
        }
    }
}
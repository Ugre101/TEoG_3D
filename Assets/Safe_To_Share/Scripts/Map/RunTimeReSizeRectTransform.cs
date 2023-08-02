using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Safe_To_Share.Scripts.Map {
    public sealed class RunTimeReSizeRectTransform : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        static Vector2 newSize;
        [SerializeField] float minSize = 100f, maxSize = 600f;
        [SerializeField] bool invertX, invertY;
        [SerializeField] RectTransform rect;

        Vector2 lastPos;

        Vector2 startPos;

        void Start() {
            if (newSize != Vector2.zero)
                rect.sizeDelta = newSize;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            startPos = eventData.position;
            lastPos = startPos;
        }

        public void OnDrag(PointerEventData eventData) {
            ChangeAccordingToMousePos(eventData.position, lastPos);
            lastPos = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData) => newSize = rect.sizeDelta;
        public event Action ChangedSize;

        void ChangeAccordingToMousePos(Vector2 mousePos, Vector2 prevPos) {
            var diff = prevPos - mousePos;
            var x = invertX ? -diff.x : diff.x;
            var y = invertY ? -diff.y : diff.y;
            var didXChangeMost = Mathf.Abs(y) < Mathf.Abs(x);
            var mostChanged = didXChangeMost ? x : y;
            ChangeRectSize(mostChanged);
        }

        void ChangeRectSize(float changeValue) {
            var value = Mathf.Clamp(rect.sizeDelta.x + changeValue, minSize, maxSize);
            Vector2 temp = new(value, value);
            rect.sizeDelta = temp;
            ChangedSize?.Invoke();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory {
    public class InventorySlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                     IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] TextMeshProUGUI amount;
        [SerializeField] RectTransform rect;
        [SerializeField] Image iconSprite;
        [SerializeField] float doubleClickResetTime = 2f;
        [SerializeField] new Transform transform;

        bool blockHoverInfo;
        protected InventoryItem invItem;
        float lastClick;

        bool loaded;
        Item loadedItem;
        Coroutine loadItemOp;
        Transform orgParent;
        protected InventorySlot slot;

        Sprite Sprite {
            set => iconSprite.sprite = value;
        }

        void OnDisable() {
            StopCoroutine(loadItemOp);
            if (invItem != null)
                invItem.AmountChange -= SetAmount;
        }

        public virtual void OnBeginDrag(PointerEventData eventData) {
            blockHoverInfo = true;
            StopShowing?.Invoke();
            orgParent = transform.parent;
            var orgSize = rect.rect.size;
            SetAnchor(rect, 0.5f, 0.5f);
            rect.sizeDelta = orgSize;
            transform.SetParent(orgParent.parent.parent.parent, false);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData) => transform.position = eventData.position;

        public virtual void OnEndDrag(PointerEventData eventData) {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var o in results) {
                if (!o.gameObject.TryGetComponent(out InventorySlot newSlot)) continue;
                ResetPosition();
                newSlot.MoveTo(invItem, slot);
                return;
            }

            ResetPosition();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (!blockHoverInfo)
                ShowItem?.Invoke(loadedItem, transform.position);
        }

        public void OnPointerExit(PointerEventData eventData) => StopShowing?.Invoke();
        public event Action<Item, InventoryItem, InventorySlot> Use;

        protected void ResetPosition() {
            transform.SetParent(orgParent);
            transform.position = orgParent.position;
            SetAnchor(rect, 0f, 1f);
            rect.offsetMax = new Vector2(-10, -10);
            rect.offsetMin = new Vector2(10, 10);
            blockHoverInfo = false;
            lastClick = 0;
        }

        static void SetAnchor(RectTransform rect, float minAnchor, float maxAnchor) {
            rect.anchorMin = new Vector2(minAnchor, minAnchor);
            rect.anchorMax = new Vector2(maxAnchor, maxAnchor);
        }

        void SetAmount(int value) => amount.text = value.ToString();

        public void Setup(InventoryItem parInvItem, InventorySlot inventorySlot) {
            slot = inventorySlot;
            gameObject.SetActive(true);
            invItem = parInvItem;
            SetAmount(parInvItem.Amount);
            loadItemOp = StartCoroutine(LoadItem());
            invItem.AmountChange += SetAmount;
        }

        IEnumerator LoadItem() {
            var op = Addressables.LoadAssetAsync<Item>(invItem.ItemGuid);
            yield return op;
            loaded = true;
            loadedItem = op.Result;
            Sprite = op.Result.Icon;
        }

        public void DoubleClick() {
            if (!loaded) return;
            var newClick = Time.unscaledTime;
            if (newClick - lastClick < doubleClickResetTime) {
                Use?.Invoke(loadedItem, invItem, slot);
                SetAmount(invItem.Amount);
            }

            lastClick = newClick;
        }

        public static event Action<Item, Vector2> ShowItem;
        public static event Action StopShowing;

        public void Clear() {
            if (invItem != null)
                invItem.AmountChange -= SetAmount;
            loaded = false;
            gameObject.SetActive(false);
        }
    }
}
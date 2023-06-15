using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Farming.UI {
    public class
        PlantOptionButton : Selectable //,IDeselectHandler,ISelectHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] Image icon;

        //       InventoryItem item;
        string guid;
        Inventory inventory;
        Plant plant;
        public static event Action<Plant, Inventory, string, PlantOptionButton> ShowPlacement;

        public void Setup(Plant plant, string handleKey, Inventory inventory1) {
            this.plant = plant;
            // this.item = inventoryItem;
            guid = handleKey;
            inventory = inventory1;
            icon.sprite = plant.Icon;
        }

        public override void OnDeselect(BaseEventData eventData) {
            base.OnDeselect(eventData);
        }

        public override void OnSelect(BaseEventData eventData) {
            ShowPlacement?.Invoke(plant, inventory, guid, this);
            base.OnSelect(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData) {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData) {
            base.OnPointerExit(eventData);
        }
    }
}
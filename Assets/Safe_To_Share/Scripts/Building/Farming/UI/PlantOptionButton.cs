using System;
using System.Management.Instrumentation;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public class PlantOptionButton : Selectable //,IDeselectHandler,ISelectHandler,IPointerEnterHandler,IPointerExitHandler
    {
        public static event Action<Plant,Inventory,InventoryItem,PlantOptionButton> ShowPlacement;
        Plant plant;

        InventoryItem item;

        Inventory inventory;
        
        [SerializeField] Image icon;

        public void Setup(Plant plant, InventoryItem inventoryItem, Inventory inventory1)
        {
            this.plant = plant;
            this.item = inventoryItem;
            this.inventory = inventory1;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            ShowPlacement?.Invoke(plant,inventory,item,this);
            base.OnSelect(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }
    }
}
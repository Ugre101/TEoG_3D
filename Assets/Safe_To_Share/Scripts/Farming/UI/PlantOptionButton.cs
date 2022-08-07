using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public class PlantOptionButton : Selectable //,IDeselectHandler,ISelectHandler,IPointerEnterHandler,IPointerExitHandler
    {
        public static event Action<Plant,InventoryItem> ShowPlacement;
        Plant plant;

        InventoryItem item;
        
        [SerializeField] Image icon;

        public void Setup(Plant plant,InventoryItem inventory)
        {
            this.plant = plant;
            this.item = inventory;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            ShowPlacement?.Invoke(plant,item);
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
using System;
using System.Collections.Generic;
using System.Text;
using Character.PlayerStuff.Currency;
using Character.StatsStuff;
using Currency.UI;
using Items;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.Menus.Inventory
{
    public class InventoryMenu : GameMenu
    {
        [SerializeField] Transform content;
        [SerializeField] HaveGold haveGold;
        [SerializeField] InventorySlot[] preInstancedSlots;
        [SerializeField] InventorySlot slot;
        [SerializeField] TextMeshProUGUI stats;
        bool firstUse = true;
        protected Dictionary<Vector2, InventorySlot> Slots = new();
        Queue<InventorySlot> slotsPool;


        void OnEnable()
        {
            if (firstUse)
            {
                firstUse = false;
                SetupSlotPool();
                Setup();
            }

            AddItems();
            ShowStats();
            haveGold.GoldChanged(PlayerGold.GoldBag.Gold);
            foreach (CharStat charStat in Player.Stats.GetCharStats.Values)
                charStat.StatDirtyEvent += ReDraw;
            InventorySlotItem.Use += UseItem;
        }

        void OnDisable()
        {
            foreach (CharStat charStat in Player.Stats.GetCharStats.Values)
                charStat.StatDirtyEvent -= ReDraw;
            InventorySlotItem.Use -= UseItem;
        }

#if UNITY_EDITOR
        void OnValidate() => preInstancedSlots = content.GetComponentsInChildren<InventorySlot>(true);
#endif

        InventorySlot GetSlot()
        {
            if (slotsPool.Count > 0)
            {
                var getSlot = slotsPool.Dequeue();
                getSlot.gameObject.SetActive(true);
                return getSlot;
            }

            return Instantiate(slot, content);
        }

        void ReDraw() => ShowStats();

        void ShowStats()
        {
            StringBuilder sb = new();
            foreach ((CharStatType key, CharStat value) in Player.Stats.GetCharStats)
                sb.AppendLine($"{key} {value.Value}");
            stats.text = sb.ToString();
        }

        public void Setup()
        {
            // Reset
            Slots = new Dictionary<Vector2, InventorySlot>();
            // Add slots
            for (int x = 0; x < Player.Inventory.InventorySize.x; x++)
            for (int y = 0; y < Player.Inventory.InventorySize.y; y++)
                AddInventorySlot(x, y);
        }

        void AddInventorySlot(int x, int y)
        {
            InventorySlot newSlot = GetSlot();
            newSlot.Setup(new Vector2(x, y));
            newSlot.MovedItem += MoveItem;
            Slots.Add(newSlot.Position, newSlot);
        }

        void MoveItem(InventoryItem item, Vector2 pos)
        {
            Slots[item.Position].ClearItem();
            if (Player.Inventory.MoveItem(item, pos, out InventoryItem oldItem))
            {
                AddInventoryItem(oldItem);
                Slots[pos].ClearItem();
            }

            AddInventoryItem(item);
        }

        void AddItems()
        {
            foreach (var pair in Slots)
                pair.Value.ClearItem();
            foreach (InventoryItem invItem in Player.Inventory.Items)
                AddInventoryItem(invItem);
        }

        void SetupSlotPool()
        {
            slotsPool = new Queue<InventorySlot>();
            foreach (var inventorySlot in preInstancedSlots)
            {
                inventorySlot.ClearItem();
                inventorySlot.gameObject.SetActive(false);
                slotsPool.Enqueue(inventorySlot);
            }
        }


        void AddInventoryItem(InventoryItem invItem) => Slots[invItem.Position].AddItem(invItem);

        public static event Action StopHoverInfo;

        void UseItem(Item loaded, InventoryItem item)
        {
            loaded.Use(Player);
            if (Player.Inventory.UseLoadedItem(loaded, item.Position))
                InventoryClearItemOnCord(item.Position);
            if (loaded.UpdateInventoryAfterUse)
                AddItems();
            holder.UpdateAvatar();
            holder.HeightsChange(Player.Body.Height.Value);
        }

        void InventoryClearItemOnCord(Vector2 pos)
        {
            Slots[pos].ClearItem();
            StopHoverInfo?.Invoke();
        }
    }
}
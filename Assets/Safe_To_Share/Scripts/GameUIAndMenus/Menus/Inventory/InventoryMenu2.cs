using System;
using System.Text;
using Character.GenderStuff;
using Character.PlayerStuff.Currency;
using Character.Race.Races;
using Character.StatsStuff;
using Currency.UI;
using GameUIAndMenus;
using Items;
using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory
{
    public class InventoryMenu2 : GameMenu
    {
        [SerializeField] InventoryBah playerInventory;
        [SerializeField] InventoryBah secondaryInventory;
        [SerializeField] TextMeshProUGUI stats;
        [SerializeField] HaveGold haveGold;
        [SerializeField] CraftingTable craftingTable;
        [SerializeField] GameObject altStuff;
        void OnEnable()
        {
            playerInventory.Setup(Player.Inventory);
            craftingTable.Setup(Player);
            ShowStats();
            haveGold.GoldChanged(PlayerGold.GoldBag.Gold);
            foreach (var charStat in Player.Stats.GetCharStats.Values)
                charStat.StatDirtyEvent += ShowStats;
            // InventorySlotItem.Use += UseItem;
            InventoryBah.Use += InventoryBahOnUse;
        }

        void InventoryBahOnUse(Item loaded)
        {
            loaded.Use(Player);
            if (loaded.UpdateInventoryAfterUse)
                playerInventory.AddItems();
        }


        void OnDisable()
        {
            //    InventorySlotItem.Use -= UseItem;
            InventoryBah.Use -= InventoryBahOnUse;
            altStuff.SetActive(true);
            secondaryInventory.gameObject.SetActive(false);
        }
        public static event Action StopHoverInfo;


        void UseItem(Item loaded, InventoryItem item, InventorySlot arg3)
        {
            BasicRace startRace = Player.RaceSystem.Race;
            Gender startGender = Player.Gender;
            loaded.Use(Player);
            if (Player.Inventory.UseLoadedItem(loaded, item.Position))
            {
                arg3.ClearItem();
                StopHoverInfo?.Invoke();
            }
            if (loaded.UpdateInventoryAfterUse)
                playerInventory.AddItems();
        }


        public void ShowStats()
        {
            StringBuilder sb = new();
            foreach ((CharStatType key, CharStat value) in Player.Stats.GetCharStats)
                sb.AppendLine($"{key} {value.Value}");
            stats.text = sb.ToString();
        }

        public void SetupSecondaryInventory(Items.Inventory inventory)
        {
            altStuff.SetActive(false);
            secondaryInventory.gameObject.SetActive(true);
            secondaryInventory.Setup(inventory);
        }
    }
}
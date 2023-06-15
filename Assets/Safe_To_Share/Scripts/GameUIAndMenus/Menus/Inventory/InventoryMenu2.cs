using System;
using System.Text;
using Character.PlayerStuff.Currency;
using Currency.UI;
using Items;
using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory {
    public sealed class InventoryMenu2 : GameMenu {
        [SerializeField] InventoryBah playerInventory;
        [SerializeField] InventoryBah secondaryInventory;
        [SerializeField] TextMeshProUGUI stats;
        [SerializeField] HaveGold haveGold;
        [SerializeField] CraftingTable craftingTable;
        [SerializeField] GameObject altStuff;

        void OnEnable() {
            playerInventory.Setup(Player.Inventory);
            craftingTable.Setup(Player);
            ShowStats();
            haveGold.GoldChanged(PlayerGold.GoldBag.Gold);
            foreach (var charStat in Player.Stats.GetCharStats.Values)
                charStat.StatDirtyEvent += ShowStats;
            // InventorySlotItem.Use += UseItem;
            InventoryBah.Use += InventoryBahOnUse;
        }


        void OnDisable() {
            //    InventorySlotItem.Use -= UseItem;
            InventoryBah.Use -= InventoryBahOnUse;
            altStuff.SetActive(true);
            secondaryInventory.gameObject.SetActive(false);
        }

        void InventoryBahOnUse(Item loaded) {
            loaded.Use(Player);
            if (loaded.UpdateInventoryAfterUse)
                playerInventory.AddItems();
        }

        public static event Action StopHoverInfo;


        void UseItem(Item loaded, InventoryItem item, InventorySlot arg3) {
            var startRace = Player.RaceSystem.Race;
            var startGender = Player.Gender;
            loaded.Use(Player);
            if (Player.Inventory.UseLoadedItem(loaded, item.Position)) {
                arg3.ClearItem();
                StopHoverInfo?.Invoke();
            }

            if (loaded.UpdateInventoryAfterUse)
                playerInventory.AddItems();
        }


        public void ShowStats() {
            StringBuilder sb = new();
            foreach ((var key, var value) in Player.Stats.GetCharStats)
                sb.AppendLine($"{key} {value.Value}");
            stats.text = sb.ToString();
        }

        public void SetupSecondaryInventory(Items.Inventory inventory) {
            altStuff.SetActive(false);
            secondaryInventory.gameObject.SetActive(true);
            secondaryInventory.Setup(inventory);
        }
    }
}
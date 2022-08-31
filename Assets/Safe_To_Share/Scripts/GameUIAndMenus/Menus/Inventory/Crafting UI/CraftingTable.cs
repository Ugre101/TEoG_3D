using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Character.PlayerStuff;
using Items;
using Safe_To_Share.Scripts.Character.Items.Crafting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI
{
    public class CraftingTable : MonoBehaviour
    {
        [SerializeField] CraftingRecipesDict craftingDictionary;
        [SerializeField] CraftingInventorySlot slot1, slot2;
        [SerializeField] CraftingResult result;
        [SerializeField] InventoryBah inventoryBag;
        CraftingRecipe canMake;
        bool hasValidRecipe;
        string item1, item2;

        Dictionary<string, AsyncOperationHandle<Item>> loadedDictionary = new();

        Player player;
        Coroutine routine;

        void OnEnable()
        {
            slot1.ReceivedItem += Slot1OnReceivedItem;
            slot1.ClearedItem += Slot1OnClearedItem;
            slot2.ReceivedItem += Slot2OnReceivedItem;
            slot2.ClearedItem += Slot2OnClearedItem;
        }

        void OnDisable()
        {
            slot1.ReceivedItem -= Slot1OnReceivedItem;
            slot1.ClearedItem -= Slot1OnClearedItem;
            slot2.ReceivedItem -= Slot2OnReceivedItem;
            slot2.ClearedItem -= Slot2OnClearedItem;
            if (result.StillHasItem(out var items, out var amount)) 
                player.Inventory.AddItem(items, amount);

            foreach (var (key, handle) in loadedDictionary) 
                Addressables.Release(handle);

            loadedDictionary = new Dictionary<string, AsyncOperationHandle<Item>>();
        }

        public void Setup(Player parPlayer) => player = parPlayer;
        
        public void OpenCraftingTable()
        {
            gameObject.SetActive(true);
            slot1.ClearItem();
            slot2.ClearItem();
            result.ShowQuestionMark();
        }

        void Slot1OnClearedItem()
        {
            item1 = string.Empty;
            ClearValidRecipe();
        }
        void Slot2OnClearedItem()
        {
            item2 = string.Empty;
            ClearValidRecipe();
        }
        void ClearValidRecipe()
        {
            hasValidRecipe = false;
            canMake = null;
            result.ShowQuestionMark();
        }
        void Slot1OnReceivedItem(InventoryItem obj)
        {
            item1 = obj.ItemGuid;
            if (!string.IsNullOrEmpty(item2))
                CheckForValidRecipe();
        }

        void CheckForValidRecipe()
        {
            if (string.IsNullOrEmpty(item1) || string.IsNullOrEmpty(item2)) return;
            hasValidRecipe = craftingDictionary.TryGetResult(item1, item2, out var recipe);
            canMake = recipe;
            if (!hasValidRecipe)
            {
                return;
            }
            if (!loadedDictionary.TryGetValue(canMake.Result.guid, out var handle))
            {
                handle = Addressables.LoadAssetAsync<Item>(recipe.Result.guid);
                loadedDictionary.Add(canMake.Result.guid, handle);
            }

            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(ShowResult(handle));
        }

        void Slot2OnReceivedItem(InventoryItem obj)
        {
            item2 = obj.ItemGuid;
            if (!string.IsNullOrEmpty(item1))
                CheckForValidRecipe();
        }


        public void LeaveCraftingTable()
        {
            gameObject.SetActive(false);
            slot1.ClearItem();
            slot2.ClearItem();
        }

        IEnumerator ShowResult(AsyncOperationHandle<Item> handle)
        {
            if (!hasValidRecipe) yield break;
            result.ShowQuestionMark();
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded) result.AddResult(handle.Result);
        }

        bool crafting = false;
        public async void Craft()
        {
            if (crafting) return;
            if (!hasValidRecipe) return;
            crafting = true;
            if (player.Inventory.AddAndReturnIfNewItem(canMake.Result.guid,out var newItem))
            {
                inventoryBag.AddInventoryItem(newItem);
            }
            var op1  = await player.Inventory.LowerItemAmountAndReturnIfStillHave(item1);
            if (!op1)
            {
                ClearValidRecipe();
                slot1.ClearOrgItemThenClear();
            }

            var op2 = await player.Inventory.LowerItemAmountAndReturnIfStillHave(item2);
            if (!op2)
            {
                if (op1)
                    ClearValidRecipe();
                slot2.ClearOrgItemThenClear();
            }
        
            crafting = false;
        }
    }
}
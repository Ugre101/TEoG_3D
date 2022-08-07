using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public class ShowPlantOptions : MonoBehaviour
    {
        [SerializeField] Transform content;
        [SerializeField] PlantOptionButton prefab;
        Inventory inventory;

        public void Setup(Inventory inventory)
        {
            this.inventory = inventory;
            content.KillChildren();
            CheckForSeeds();
        }
        
        void CheckForSeeds()
        {
            foreach (InventoryItem inventoryItem in inventory.Items)
            {
                Addressables.LoadAssetAsync<Item>(inventoryItem.ItemGuid).Completed += (item) =>
                {
                    if (item.Result is Seed seed && seed.SeedFor != null)
                    {
                        var option = Instantiate(prefab, content);
                        option.Setup(seed.SeedFor, inventoryItem);
                    }
                };
            }
            
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public class ShowPlantOptions : MonoBehaviour
    {
        [SerializeField] Transform content;
        [SerializeField] PlantOptionButton prefab;
        [SerializeField] TextMeshProUGUI noSeedText;
        Inventory inventory;

        bool noSeed;

        void OnEnable() => ShowPlantPlacement.UpdateSeedsOptions += SeedsLeft;

        void OnDisable() => ShowPlantPlacement.UpdateSeedsOptions -= SeedsLeft;

        void SeedsLeft() => noSeedText.gameObject.SetActive(content.childCount > 0);

        public void Setup(Inventory inventory)
        {
            this.inventory = inventory;
            content.KillChildren();
            noSeed = true;
            StartCoroutine(CheckForSeeds());
        }
        
        IEnumerator CheckForSeeds()
        {
            List<KeyValuePair< InventoryItem,AsyncOperationHandle<Item>>> ops = new();
            foreach (InventoryItem inventoryItem in inventory.Items)
            {
                var op = Addressables.LoadAssetAsync<Item>(inventoryItem.ItemGuid);
                ops.Add(new KeyValuePair<InventoryItem, AsyncOperationHandle<Item>>(inventoryItem,op));
            }

            foreach (var handle in ops)
            {
                yield return handle.Value;
                Item item = handle.Value.Result;
                if (item is Seed seed && seed.SeedFor != null)
                {
                    var option = Instantiate(prefab, content);
                    option.Setup(seed.SeedFor, handle.Key,inventory);
                    noSeed = false;
                }
            }

            noSeedText.gameObject.SetActive(noSeed);
        }

        
    }
}
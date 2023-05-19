using System.Collections;
using System.Collections.Generic;
using Items;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Farming.UI
{
    public sealed class ShowPlantOptions : MonoBehaviour
    {
        [SerializeField] Transform content;
        [SerializeField] PlantOptionButton prefab;
        [SerializeField] TextMeshProUGUI noSeedText;
        Inventory inventory;

        bool noSeed;

        void OnEnable() => ShowPlantPlacement.UpdateSeedsOptions += SeedsLeft;

        void OnDisable() => ShowPlantPlacement.UpdateSeedsOptions -= SeedsLeft;

        void SeedsLeft() => noSeedText.gameObject.SetActive(content.childCount > 0);

        public void Setup(Inventory parInventory)
        {
            inventory = parInventory;
            content.KillChildren();
            noSeed = true;
            StartCoroutine(CheckForSeeds());
        }
        
        IEnumerator CheckForSeeds()
        {
            Dictionary<string, AsyncOperationHandle<Item>> dict = new();
            foreach (var inventoryItem in inventory.Items)
            {
                if (dict.ContainsKey(inventoryItem.ItemGuid))
                    continue;
                dict.TryAdd(inventoryItem.ItemGuid, Addressables.LoadAssetAsync<Item>(inventoryItem.ItemGuid));
            }
            
            foreach (var handle in dict)
            {
                yield return handle.Value;
                Item item = handle.Value.Result;
                if (item is Seed seed && seed.SeedFor != null)
                {
                    var option = Instantiate(prefab, content);
                    option.Setup(seed.SeedFor,handle.Key,inventory);
                    noSeed = false;
                }
            }
            noSeedText.gameObject.SetActive(noSeed);
        }

        
    }
}
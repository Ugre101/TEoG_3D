using System;
using Character.PlayerStuff;
using CustomClasses;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Safe_To_Share.Scripts.Map.Pick_Ups {
    public sealed class MineAbleMineral : MonoBehaviour, IInteractable {
        [SerializeField, Range(0f, 1f),] float spawnChance = 1f;
        [SerializeField, Range(1, 3),] int minAmount = 1;
        [SerializeField, Range(1, 9),] int maxAmount = 3;
        [SerializeField] DropSerializableObject<Item> item;
        int amount;

        void Start() {
            if (spawnChance < 1f && Random.value > spawnChance)
                Destroy(gameObject);
            else
                amount = Random.Range(minAmount, maxAmount);
        }


        public string HoverText(Player player) => "Take";

        public void DoInteraction(Player player) {
            player.Inventory.AddItem(item.guid, amount);
            RemoveIInteractableHit?.Invoke();
            Destroy(gameObject);
        }


        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
    }
}
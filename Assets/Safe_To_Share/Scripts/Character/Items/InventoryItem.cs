using System;
using UnityEngine;

namespace Items {
    [Serializable]
    public class InventoryItem {
        [SerializeField] string itemGuid;
        [SerializeField] Vector2 position;
        [SerializeField] int amount;

        public InventoryItem(string itemGuid, int amount, Vector2 position) {
            this.itemGuid = itemGuid;
            Amount = amount;
            Position = position;
        }


        public string ItemGuid => itemGuid;

        public Vector2 Position {
            get => position;
            set => position = value;
        }

        public int Amount {
            get => amount;
            set {
                amount = value;
                AmountChange?.Invoke(amount);
            }
        }

        public event Action<int> AmountChange;
    }
}
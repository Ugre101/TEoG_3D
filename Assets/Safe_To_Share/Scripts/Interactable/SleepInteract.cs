﻿using System;
using Character;
using Character.EnemyStuff;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Currency;
using Safe_To_Share.Scripts.Map.Spawner;
using UnityEngine;

namespace Safe_To_Share.Scripts.Interactable {
    public sealed class SleepInteract : MonoBehaviour, IInteractable {
        [SerializeField] bool hasCost;
        [SerializeField] int cost;
        [SerializeField] int sleepQuality = 150;

        public string HoverText(Player player) =>
            hasCost
                ? PlayerGold.GoldBag.CanAfford(cost) ? $"Sleep {cost}g" : $"Sleep need {cost}g"
                : "Sleep";

        public void DoInteraction(Player player) {
            if (!hasCost) {
                player.Sleep(sleepQuality);
                return;
            }

            if (!player.TryToBuy(cost))
                return;
            player.Sleep(sleepQuality);
            SavedEnemies.ClearEnemies();
            SpawnZones.Instance.ClearEnemies();
            UpdateHoverText?.Invoke(this);
        }

        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
    }
}
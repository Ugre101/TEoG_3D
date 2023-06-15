﻿using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming.UI {
    public sealed class FarmCanvas : MonoBehaviour {
        [SerializeField] ShowPlantOptions showPlantOptions;

        public void Open(Player player) {
            gameObject.SetActive(true);
            GameUIManager.TriggerHideGameUI(true);
            showPlantOptions.gameObject.SetActive(true);
            showPlantOptions.Setup(player.Inventory);
        }

        public void ToggleBuildMenu() => showPlantOptions.gameObject.SetActive(!showPlantOptions.gameObject.activeSelf);


        public void Close() {
            gameObject.SetActive(false);
            GameUIManager.TriggerHideGameUI(false);
        }
    }
}
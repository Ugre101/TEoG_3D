using System;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore {
    public sealed class StomachVoreOrganContainerInfo : VoreOrganContainerInfo {
        [SerializeField] VoreOrganCapacityInfo prefab;
        public static event Action ShowStomachSettings;

        public void Setup(string title, VoreOrgan voreOrgan, float capcity) {
            putHere.KillChildren();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ShowMe);
            Instantiate(prefab, putHere).Setup(title, voreOrgan, capcity);
        }

        protected override void ShowMe() => ShowStomachSettings?.Invoke();
    }
}
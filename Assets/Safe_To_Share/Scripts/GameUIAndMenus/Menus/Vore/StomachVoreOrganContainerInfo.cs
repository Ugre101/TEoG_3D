using System;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace GameUIAndMenus.Menus.Vore
{
    public class StomachVoreOrganContainerInfo : VoreOrganContainerInfo
    {
        public static event Action ShowStomachSettings;
        [SerializeField] VoreOrganCapacityInfo prefab;

        public void Setup(string title, VoreOrgan voreOrgan, float capcity)
        {
            putHere.KillChildren();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ShowMe);
            Instantiate(prefab, putHere).Setup(title, voreOrgan, capcity);
        }

        protected override void ShowMe() => ShowStomachSettings?.Invoke();
    }
}
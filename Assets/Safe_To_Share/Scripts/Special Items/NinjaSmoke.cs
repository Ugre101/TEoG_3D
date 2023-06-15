using Character;
using Items;
using Safe_To_Share.Scripts.GameUIAndMenus;
using Safe_To_Share.Scripts.Holders;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Special_Items {
    [CreateAssetMenu(menuName = "Items/Special Items/Create NinjaSmoke", fileName = "NinjaSmoke", order = 0)]
    public sealed class NinjaSmoke : Item {
        [SerializeField] SceneTeleportExit exit;

        public override void Use(BaseCharacter user) {
            var holder = PlayerHolder.Instance;
            var canvas = FindObjectOfType<GameCanvas>(); // I don't like this, but don't know how to improve
            if (holder == null || canvas == null) return;
            canvas.CloseMenus();
            SceneLoader.Instance.TeleportToExit(holder, exit);
        }
    }
}
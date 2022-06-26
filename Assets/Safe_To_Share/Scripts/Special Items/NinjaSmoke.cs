using AvatarStuff.Holders;
using Character;
using GameUIAndMenus;
using Items;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Special_Items
{
    [CreateAssetMenu(menuName = "Items/Special Items/Create NinjaSmoke", fileName = "NinjaSmoke", order = 0)]
    public class NinjaSmoke : Item
    {
        [SerializeField] SceneTeleportExit exit;

        public override void Use(BaseCharacter user)
        {
            var holder = PlayerHolder.Instance;
            var canvas = FindObjectOfType<GameCanvas>(); // I don't like this, but don't know how to improve
            if (holder != null && canvas != null)
            {
                canvas.CloseMenus();
                SceneLoader.Instance.TeleportToExit(holder, exit);
            }
        }
    }
}
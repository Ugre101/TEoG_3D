using Character;
using Character.PlayerStuff;
using Items;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Special_Items {
    [CreateAssetMenu(menuName = "Items/Special Items/Create TeleportStone", fileName = "TeleportStone", order = 0)]
    public sealed class TeleportStone : Item {
        public override void Use(BaseCharacter user) {
            if (user is Player player)
                SceneLoader.Instance.GoHome(player);
        }
    }
}
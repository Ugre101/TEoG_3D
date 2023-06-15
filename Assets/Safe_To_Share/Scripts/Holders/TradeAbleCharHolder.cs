using Character.Npc;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders.Npc;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders {
    [RequireComponent(typeof(Collider))]
    public sealed class TradeAbleCharHolder : NpcHolder {
        [SerializeField] TradeAbleCharacter tradeAbleCharacter;


#if UNITY_EDITOR

        void OnValidate() {
            if (gameObject.layer == LayerMask.NameToLayer("Interactable")) return;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            print($"Npc {gameObject.name} wasn't on correct layer");
        }
#endif
        public override string HoverText(Player player) => tradeAbleCharacter.HoverText(player);

        public override void DoInteraction(Player player) => tradeAbleCharacter.DoInteraction(player);
    }
}
using System;
using AvatarStuff.Holders.Npc;
using Character.Npc;
using Character.PlayerStuff;
using UnityEngine;

namespace Holders
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class TradeAbleCharHolder : NpcHolder
    {
        [SerializeField] TradeAbleCharacter tradeAbleCharacter;
        public override string HoverText(Player player) => tradeAbleCharacter.HoverText(player);

        public override void DoInteraction(Player player) => tradeAbleCharacter.DoInteraction(player);


#if UNITY_EDITOR
        
        void OnValidate()
        {
            if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
            {
                gameObject.layer = LayerMask.NameToLayer("Interactable");
                print($"Npc {gameObject.name} wasn't on correct layer");
            }
        }
#endif
    }
}
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders.Npc;
using UnityEngine;

namespace Dialogue
{
    public sealed class NpcWithDialogue : NpcHolder
    {
        [SerializeField] BaseDialogue dialogue;
        public override string HoverText(Player player) => "Talk";

        public override void DoInteraction(Player player) => dialogue.StartTalking();
    }
}
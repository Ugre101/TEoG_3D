using AvatarStuff.Holders.Npc;
using Character.PlayerStuff;
using UnityEngine;

namespace Dialogue
{
    public class NpcWithDialogue : NpcHolder
    {
        [SerializeField] BaseDialogue dialogue;
        public override string HoverText(Player player) => "Talk";

        public override void DoInteraction(Player player) => dialogue.StartTalking();
    }
}
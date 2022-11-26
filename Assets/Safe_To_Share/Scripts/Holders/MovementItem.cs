using System.Collections.Generic;
using AvatarStuff.Holders;
using Character;
using Character.PlayerStuff;
using Character.StatsStuff.Mods;
using Items;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Special_Items
{
    [CreateAssetMenu(menuName = "Items/Special Items/Create Movement Item", fileName = "Movement Item", order = 0)]
    public class MovementItem : Item
    {
        [SerializeField] List<TempIntMod> sprintMods = new();
        [SerializeField] List<TempIntMod> jumpStrength = new();
        public List<TempIntMod> SprintMods => sprintMods;

        public List<TempIntMod> JumpStrength => jumpStrength;

        public override void Use(BaseCharacter user)
        {
            if (user is Player player)
                PlayerHolder.Instance.MoveModHandler.AddItemEffects(this);
        }
    }
}
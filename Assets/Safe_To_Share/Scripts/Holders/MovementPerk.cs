using System.Collections.Generic;
using Character.LevelStuff;
using Character.PlayerStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace AvatarStuff.Holders
{
    
    [CreateAssetMenu(menuName = "Character/Movement/Perk", fileName = "MovementPerk", order = 0)]
    public class MovementPerk : BasicPerk
    {
        [SerializeField] int extraJumps;
        [SerializeField] float extraJumpStrength;
        [SerializeField] List<TempIntMod> tempJumpStrength = new();
        [SerializeField] float sprintBoost;
        [SerializeField] List<TempIntMod> tempSprintBoost = new();
        [SerializeField] IntMod[] swimSpeedMods;
        
        
        public override PerkType PerkType => PerkType.Movement;
        public float SprintBoost => sprintBoost;

        public int ExtraJumps => extraJumps;

        public float ExtraJumpStrength => extraJumpStrength;

        public IEnumerable<TempIntMod> TempJumpStrength => tempJumpStrength;

        public IEnumerable<TempIntMod> TempSprintBoost => tempSprintBoost;

        public IEnumerable<IntMod> SwimSpeedMods => swimSpeedMods;

        public void GainMovementMods(PlayerHolder holder) => holder.MoveModHandler.AddPerkEffects(this);
    }
}
using System;
using System.Collections.Generic;
using Character.StatsStuff.Mods;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Movement.HoverMovement;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders
{
    public sealed class MovementModHandler : MoveStats
    {
        [SerializeField] Safe_To_Share.Scripts.Movement.HoverMovement.Movement movement;

        bool firstUse = true;
        [SerializeField] BaseFloatStat jumpCount = new(1);
        [SerializeField] BaseFloatStat jumpStrength = new(2f);
        [SerializeField] BaseFloatStat sprintSpeed = new(1.5f);
        [SerializeField] BaseFloatStat swimSpeed = new(10f);
        [SerializeField] BaseFloatStat walkSpeed = new(12f);
        public void Reset()
        {
            FirstSetup();
            DateSystem.TickHour -= TickHour;
            DateSystem.TickHour += TickHour;
        }
        void FirstSetup()
        {
            firstUse = false;
        }

        public void AddWalkTempEffect(TempIntMod tempMod)
        {
            if (firstUse)
                FirstSetup();
            walkSpeed.Mods.AddTempStatMod(tempMod);
            SetValues();
        }

        public void RemoveWalkTempEffect(TempIntMod tempMod)
        {
            if (firstUse)
                FirstSetup();
            walkSpeed.Mods.RemoveTempStatMod(tempMod);
            SetValues();
        }

        public void AddPerkEffects(MovementPerk perk)
        {
            if (firstUse)
                FirstSetup();
            sprintSpeed.BaseValue += perk.SprintBoost;
            jumpCount.BaseValue += perk.ExtraJumps;
            jumpStrength.BaseValue += perk.ExtraJumpStrength;
            if (perk.SwimSpeedMods != null)
                foreach (IntMod swim in perk.SwimSpeedMods)
                    swimSpeed.Mods.AddStatMod(swim);
            SetValues();
        }

        public void AddItemEffects(MovementItem movementItem)
        {
            if (firstUse)
                FirstSetup();
            foreach (var intMod in movementItem.SprintMods)
                sprintSpeed.Mods.AddTempStatMod(intMod);
            foreach (var tempIntMod in movementItem.JumpStrength)
                jumpStrength.Mods.AddTempStatMod(tempIntMod);
            SetValues();
        }

        void SetValues()
        {
        }

        public void TickHour(int ticks = 1)
        {
            jumpCount.TickHour(ticks);
            jumpStrength.TickHour(ticks);
            sprintSpeed.TickHour(ticks);
            walkSpeed.TickHour(ticks);
            swimSpeed.TickHour(ticks);
        }

        class MoveBoost
        {
            float value;
            public float Value
            {
                get
                {
                    if (clean) 
                        return value;
                    CalcValue();
                    return value;

                }
            }

            readonly List<FloatMod> mods = new();

            public void AddMod(FloatMod mod)
            {
                mods.Add(mod);
                clean = false;
            }

            public void RemoveMod(FloatMod mod)
            {
                mods.Remove(mod);
                clean = false;
            }
            void CalcValue()
            {
                var sum = 0f;
                foreach (var floatMod in mods)
                    sum += floatMod.Value;
                value = sum;
                clean = true;
            }
            

            bool clean;
        }

        readonly MoveBoost walkBoost = new();
        readonly MoveBoost swimBoost = new();

        public override void AddMod(MoveCharacter.MoveModes walking, FloatMod speedMod)
        {
            switch (walking)
            {
                case MoveCharacter.MoveModes.Walking:
                    walkBoost.AddMod(speedMod);
                    break;
                case MoveCharacter.MoveModes.Swimming:
                    swimBoost.AddMod(speedMod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(walking), walking, null);
            }
        }

        public override void RemoveMod(MoveCharacter.MoveModes walking, FloatMod speedMod)
        {
            switch (walking)
            {
                case MoveCharacter.MoveModes.Walking:
                    walkBoost.RemoveMod(speedMod);
                    break;
                case MoveCharacter.MoveModes.Swimming:
                    swimBoost.RemoveMod(speedMod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(walking), walking, null);
            }        }

        public override int MaxJumpCount => Mathf.RoundToInt(jumpCount.Value);
        public override float JumpStrength => jumpStrength.Value;
        public override float SprintMultiplier => sprintSpeed.Value;
        public override float SwimSpeed => swimSpeed.Value + swimBoost.Value;
        public override float WalkSpeed => walkSpeed.Value + walkBoost.Value;
    }
}
using Character.StatsStuff.Mods;
using Movement.ECM2.Source.Characters;
using Safe_to_Share.Scripts.CustomClasses;
using Special_Items;
using Static;
using UnityEngine;

namespace AvatarStuff.Holders
{
    public class MovementModHandler : MonoBehaviour
    {
        [SerializeField] ThirdPersonEcm2Character movement;

        [SerializeField,HideInInspector] float defaultSprint;
    
    
        [SerializeField,HideInInspector] int defaultJumpCount;
        [SerializeField,HideInInspector] float defaultJumpStrength;
        [SerializeField,HideInInspector] float defaultWalkSpeed;
        [SerializeField,HideInInspector] float defaultSwimSpeed;
        BaseFloatStat sprintSpeed;
        BaseFloatStat jumpCount;
        BaseFloatStat jumpStrength;
        BaseFloatStat walkSpeed;
        BaseFloatStat swimSpeed;

        bool firstUse = true;
        void FirstSetup()
        {
            firstUse = false;
            sprintSpeed = new BaseFloatStat(defaultSprint);
            jumpCount = new BaseFloatStat(defaultJumpCount);
            jumpStrength = new BaseFloatStat(defaultJumpStrength);
            walkSpeed = new BaseFloatStat(defaultWalkSpeed);
            swimSpeed = new BaseFloatStat(defaultSwimSpeed);
        }
        public void Reset()
        {
            FirstSetup();
            movement.jumpMaxCount = defaultJumpCount;
            movement.sprintSpeedMultiplier = defaultSprint;
            movement.jumpImpulse = defaultJumpStrength;
            movement.maxWalkSpeed = defaultWalkSpeed;
            movement.maxSwimSpeed = defaultSwimSpeed;
            DateSystem.TickHour -= TickHour;
            DateSystem.TickHour += TickHour;
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
            foreach (TempIntMod intMod in movementItem.SprintMods)
                sprintSpeed.Mods.AddTempStatMod(intMod);
            foreach (TempIntMod tempIntMod in movementItem.JumpStrength)
                jumpStrength.Mods.AddTempStatMod(tempIntMod);
            SetValues();
        }

        private void SetValues()
        {
            movement.jumpMaxCount = Mathf.RoundToInt(jumpCount.Value);
            movement.sprintSpeedMultiplier = sprintSpeed.Value;
            movement.jumpImpulse = jumpStrength.Value;
            movement.maxWalkSpeed = walkSpeed.Value;
            movement.maxSwimSpeed = swimSpeed.Value;
        }
#if UNITY_EDITOR
        void OnValidate()
        {
            if (movement == null || Application.isPlaying)
                return;
            defaultSprint = movement.sprintSpeedMultiplier;
            defaultJumpCount = movement.jumpMaxCount;
            defaultJumpStrength = movement.jumpImpulse;
            defaultWalkSpeed = movement.maxWalkSpeed;
            defaultSwimSpeed = movement.maxSwimSpeed;
        }
#endif
        public void TickHour(int ticks = 1)
        {
            jumpCount.TickHour(ticks);
            jumpStrength.TickHour(ticks);
            sprintSpeed.TickHour(ticks);
            walkSpeed.TickHour(ticks);
            swimSpeed.TickHour(ticks);
        }
    }
}
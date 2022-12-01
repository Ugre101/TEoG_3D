﻿using Character.PlayerStuff;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.Menus.Looks
{
    public class BowelBladderPressure : MonoBehaviour
    {
        [SerializeField] Slider bowel, bladder, hydration;
        public void Setup(Player player)
        {
            bowel.maxValue = player.SexualOrgans.Anals.Fluid.Value;
            bowel.value = player.SexualOrgans.Anals.Fluid.CurrentValue;

            bladder.value = player.BodyFunctions.Bladder.Pressure();
            hydration.maxValue = player.BodyFunctions.MaxHydration.Value;
            hydration.value = player.BodyFunctions.HydrationLevel;
        }
    }
}
using System;
using GameUIAndMenus;
using SaveStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IntererActiveAilments
{
    public class DoThingButtons : GameMenu
    {
        [SerializeField] NeedToShitButton shitButton;
        [SerializeField] NeedToPissButton pissButton;

        
        void Start()
        {
            Player.BodyFunctions.Bladder.BladderPressure += CheckNeedToPiss;
            Player.SexualOrgans.Anals.Fluid.CurrentValueChange += CheckNeedToShit;
            LoadManager.LoadedSave += ReCheck;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (holder != null)
            {
                shitButton.EditorSetup(holder);
                pissButton.EditorSetup(holder);
            }
        }
#endif

        void ReCheck()
        {
            
        }

        void CheckNeedToShit(float obj)
        {
            var pressure = obj / Player.SexualOrgans.Anals.Fluid.Value;
            shitButton.ValueChange(pressure);
        }


        void OnDestroy()
        {
            Player.BodyFunctions.Bladder.BladderPressure -= CheckNeedToPiss;
            Player.SexualOrgans.Anals.Fluid.CurrentValueChange -= CheckNeedToShit;
            LoadManager.LoadedSave -= ReCheck;
        }

        void CheckNeedToPiss(float obj)
        {
            pissButton.ValueChange(obj);
        }
    }
}
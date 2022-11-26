using AvatarStuff.Holders;
using GameUIAndMenus;
using Safe_To_Share.Scripts.Holders;
using SaveStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
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

        public override void SetPlayer(PlayerHolder value, GameCanvas canvas)
        {
            base.SetPlayer(value, canvas);
            shitButton.Setup(holder);
            shitButton.ValueChange(Player.SexualOrgans.Anals.Fluid.CurrentValue / Player.SexualOrgans.Anals.Fluid.Value);
            pissButton.Setup(holder);
            pissButton.ValueChange(Player.BodyFunctions.Bladder.Pressure());
        }


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
using AvatarStuff.Holders;
using Safe_To_Share.Scripts.Holders;
using SaveStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
{
    public class DoThingButtons : GameMenu
    {
        [SerializeField] NeedToShitButton shitButton;
        [SerializeField] NeedToPissButton pissButton;


        public override void SetPlayer(PlayerHolder value, GameCanvas canvas)
        {
            base.SetPlayer(value, canvas);
            shitButton.Setup(holder);
            pissButton.Setup(holder);
            ReCheck();
            ReBind();
            holder.RePlaced += ReBind;
            LoadManager.LoadedSave += ReCheck;
        }

        void ReBind()
        {
            Player.BodyFunctions.Bladder.BladderPressure += CheckNeedToPiss;
            Player.SexualOrgans.Anals.Fluid.CurrentValueChange += CheckNeedToShit;
        }

        void ReCheck()
        {
            shitButton.ValueChange(Player.SexualOrgans.Anals.Fluid.CurrentValue / Player.SexualOrgans.Anals.Fluid.Value);
            pissButton.ValueChange(Player.BodyFunctions.Bladder.Pressure());
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
            holder.RePlaced -= ReBind;
        }

        void CheckNeedToPiss(float obj) => pissButton.ValueChange(obj);
    }
}
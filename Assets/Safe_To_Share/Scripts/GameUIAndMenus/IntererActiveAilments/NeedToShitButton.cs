using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IntererActiveAilments
{
    public class NeedToShitButton : DoThingButton
    {
        public override void OnClick()
        {
            holder.TakeAShit();
        }
    }
}
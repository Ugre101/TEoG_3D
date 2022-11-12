using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IntererActiveAilments
{
    public class NeedToPissButton : DoThingButton
    {
        public override void OnClick()
        {
            holder.TakeAPiss();
        }
    }
}
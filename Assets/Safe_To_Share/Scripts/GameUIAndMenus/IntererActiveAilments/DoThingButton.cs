using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IntererActiveAilments
{
    public abstract class DoThingButton : MonoBehaviour
    {

        
        [SerializeField] protected PlayerHolder holder;
#if UNITY_EDITOR
        public void EditorSetup(PlayerHolder playerHolder) => holder = playerHolder;
#endif
        public void ValueChange(float pressure)
        {
        }

        public abstract void OnClick();
    }
}
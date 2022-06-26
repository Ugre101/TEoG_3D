using Options;
using UnityEngine;

namespace GameUIAndMenus
{
    public class HideMiniMap : MonoBehaviour
    {
        void OnEnable()
        {
            if (HideGameUIStuff.MiniMapHidden)
                transform.SleepChildren();
            else
                transform.AwakeChildren();
        }
    }
}
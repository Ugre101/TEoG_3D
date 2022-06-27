using Options;
using Safe_To_Share.Scripts.Static;
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
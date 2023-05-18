using Safe_To_Share.Scripts.Options;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
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
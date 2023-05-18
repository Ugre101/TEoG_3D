using Safe_To_Share.Scripts.Options;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public class HideLowerBar : MonoBehaviour
    {
        void OnEnable()
        {
            if (HideGameUIStuff.LowerBarsHidden)
                transform.SleepChildren();
            else
                transform.AwakeChildren();
        }
    }
}
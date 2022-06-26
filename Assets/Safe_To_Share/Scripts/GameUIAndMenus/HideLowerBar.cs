using Options;
using UnityEngine;

namespace GameUIAndMenus
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
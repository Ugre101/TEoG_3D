using Safe_To_Share.Scripts.Options;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class HideLowerBar : MonoBehaviour {
        void OnEnable() {
            if (HideGameUIStuff.LowerBarsHidden)
                transform.SleepChildren();
            else
                transform.AwakeChildren();
        }
    }
}
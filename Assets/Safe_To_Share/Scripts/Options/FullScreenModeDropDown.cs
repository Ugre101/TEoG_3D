using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.Options
{
    public sealed class FullScreenModeDropDown : MonoBehaviour
    {
        void Start()
        {
            if (TryGetComponent<TMP_Dropdown>(out var dropdown))
                dropdown.SetupTmpDropDown(Screen.fullScreenMode, ChangeMode);
        }

        static void ChangeMode(int arg0) => Screen.fullScreenMode = UgreTools.IntToEnum(arg0, Screen.fullScreenMode);
    }
}
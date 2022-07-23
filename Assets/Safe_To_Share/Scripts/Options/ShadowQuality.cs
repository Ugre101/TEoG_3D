using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.Options
{
    public class ShadowQuality : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown dropdown;
        void Start() => dropdown.SetupTmpDropDown(QualitySettings.shadowResolution, ChangeTier);

        static void ChangeTier(int arg0) =>
            QualitySettings.shadowResolution = UgreTools.IntToEnum(arg0, ShadowResolution.Medium);
    }
}
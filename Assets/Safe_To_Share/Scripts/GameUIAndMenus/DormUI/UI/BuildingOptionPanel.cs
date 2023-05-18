using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public abstract class BuildingOptionPanel : MonoBehaviour
    {
        [SerializeField] protected TMP_Dropdown dropdown;

        void OnEnable() => Setup();

        public abstract void Setup();
    }
}
using TMPro;
using UnityEngine;

namespace DormAndHome.Dorm.UI
{
    public abstract class BuildingOptionPanel : MonoBehaviour
    {
        [SerializeField] protected TMP_Dropdown dropdown;

        void OnEnable() => Setup();

        public abstract void Setup();
    }
}
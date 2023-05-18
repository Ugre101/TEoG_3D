using Character.Family;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Family
{
    public class ChildTreePrefab : MonoBehaviour
    {
        Child child;

        public void Setup(Child thisChild) => child = thisChild;
    }
}
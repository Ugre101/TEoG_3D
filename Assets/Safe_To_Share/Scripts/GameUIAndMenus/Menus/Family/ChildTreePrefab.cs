using Character.Family;
using UnityEngine;

namespace GameUIAndMenus.Menus.Family
{
    public class ChildTreePrefab : MonoBehaviour
    {
        Child child;
        
        public void Setup(Child thisChild)
        {
            child = thisChild;
        }
    }
}
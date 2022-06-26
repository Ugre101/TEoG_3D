using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.Menus.Vore
{
    public abstract class VoreOrganContainerInfo : MonoBehaviour
    {
        [SerializeField] protected Button btn;
        [SerializeField] protected Transform putHere;
        protected abstract void ShowMe();
    }
}
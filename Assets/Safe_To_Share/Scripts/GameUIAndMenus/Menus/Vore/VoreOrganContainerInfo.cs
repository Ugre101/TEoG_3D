using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public abstract class VoreOrganContainerInfo : MonoBehaviour
    {
        [SerializeField] protected Button btn;
        [SerializeField] protected Transform putHere;
        protected abstract void ShowMe();
    }
}
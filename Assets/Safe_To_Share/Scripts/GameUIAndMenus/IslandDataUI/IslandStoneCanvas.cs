using UnityEngine;

namespace GameUIAndMenus.IslandDataUI
{
    public class IslandStoneCanvas : MonoBehaviour
    {
        [SerializeField] IslandStone islandStone;
        [SerializeField] IslandStoneMenu menu;

        void Start() => islandStone.OpenStoneMenu += OpenMenu;

        void OnDestroy() => islandStone.OpenStoneMenu -= OpenMenu;

        void OpenMenu() => menu.gameObject.SetActive(true);

        public void CLoseMenus() => transform.SleepChildren();
    }
}
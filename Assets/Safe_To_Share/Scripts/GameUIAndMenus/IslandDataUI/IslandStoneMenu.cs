using Character.PlayerStuff;
using CustomClasses;
using GameUIAndMenus;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public class IslandStoneMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu, IBlockGameUI
    {
        [SerializeField] IslandStoneOption[] options;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying) return;
            options = GetComponentsInChildren<IslandStoneOption>();
        }
#endif
        public bool Block => gameObject.activeInHierarchy;

        public bool BlockIfActive()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }

        public void Open(Player player)
        {
            gameObject.SetActive(true);
            foreach (var stoneOption in options)
                stoneOption.Setup(player);
        }
    }
}
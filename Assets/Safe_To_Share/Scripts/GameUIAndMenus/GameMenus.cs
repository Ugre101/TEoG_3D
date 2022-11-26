using AvatarStuff.Holders;
using Safe_To_Share.Scripts.Holders;
using SceneStuff;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus
{
    public class GameMenus : MonoBehaviour
    {
        [SerializeField] GameCanvas gameCanvas;
        [SerializeField] Image minimap, bigMap;
        [SerializeField] GameMenu[] menus;
        void Start() => SetPlayer(SceneLoader.CurrentLocation.WorldMap);

#if UNITY_EDITOR
        void OnValidate() => menus = GetComponentsInChildren<GameMenu>(true);
#endif

        public void SetPlayer(Sprite map)
        {
            var holder = PlayerHolder.Instance;
            if (holder == null)
                return;
            foreach (GameMenu gameMenu in menus)
                gameMenu.SetPlayer(holder, gameCanvas);
            if (map != null)
            {
                minimap.sprite = map;
                bigMap.sprite = map;
            }

            gameCanvas.gameObject.SetActive(true);
        }
    }
}
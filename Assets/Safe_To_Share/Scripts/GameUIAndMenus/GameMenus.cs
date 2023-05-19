using Safe_To_Share.Scripts.Holders;
using SceneStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public sealed class GameMenus : MonoBehaviour
    {
        [SerializeField] GameCanvas gameCanvas;
        [SerializeField] Image minimap, bigMap;
        [SerializeField] GameMenu[] menus;
        void Start()
        {
            SetPlayer();
            if (!SceneLoader.Instance.InSubRealm) 
                SetupMiniMap(SceneLoader.CurrentLocation.WorldMap);
        }

#if UNITY_EDITOR
        void OnValidate() => menus = GetComponentsInChildren<GameMenu>(true);
#endif

        public void SetPlayer()
        {
            var holder = PlayerHolder.Instance;
            if (holder == null)
                return;
            foreach (GameMenu gameMenu in menus)
                gameMenu.SetPlayer(holder, gameCanvas);
            gameCanvas.gameObject.SetActive(true);
        }

        void SetupMiniMap(Sprite map)
        {
            if (map == null) return;
            minimap.sprite = map;
            bigMap.sprite = map;
        }
    }
}
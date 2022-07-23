using Map;
using SceneStuff;
using UnityEngine;

namespace GameUIAndMenus.SceneUI
{
    public class BoatTeleportMenu : GameMenu
    {
        [SerializeField] Transform content;
        [SerializeField] ShoreOptionButton btn;

        void OnEnable()
        {
            foreach (Transform child in content.transform)
                Destroy(child.gameObject);
            AddKnowPositions();
            ShoreOptionButton.LoadScene += LoadScene;
        }

        void OnDisable() => ShoreOptionButton.LoadScene -= LoadScene;
        void OnDestroy() => ShoreOptionButton.LoadScene -= LoadScene;

        void LoadScene(LocationSceneSo obj, SceneTeleportExit exit)
        {
            gameObject.SetActive(false);
            holder.gameObject.SetActive(false);
            if (SceneLoader.CurrentScene != null && SceneLoader.CurrentScene.Guid == obj.Guid)
            {
                gameCanvas.CloseMenus();
                SceneLoader.Instance.TeleportToExit(holder, exit);
            }
            else
                SceneLoader.Instance.LoadNewLocation(obj, Player, exit);
        }


        void AddKnowPositions()
        {
            foreach (KnowLocationsManager.Location knownLocation in KnowLocationsManager.KnownLocations)
                Instantiate(btn, content).Setup(knownLocation);
        }
    }
}
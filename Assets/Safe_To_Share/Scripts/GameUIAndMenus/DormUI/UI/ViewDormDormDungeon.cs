using System.Linq;
using AvatarStuff.Holders;
using Character.PlayerStuff;
using SaveStuff;
using SceneStuff;
using UnityEngine;

namespace DormAndHome.Dorm.UI
{
    public class ViewDormDormDungeon : MonoBehaviour
    {
        [SerializeField] PlayerHolder playerHolder;
        [SerializeField] DormDungeonMateButton dormMateButton;
        [SerializeField] AreYouSure areYouSure;
        [SerializeField] Transform container;
        [SerializeField] ViewSelectedDormMate viewSelected;
        void OnEnable() => Setup();
        void Fuck(DormMate obj) => SceneLoader.Instance.LoadAfterBattleFromLocation(playerHolder, obj);

        void TryKick(DormMate obj)
        {
            Instantiate(areYouSure, transform).Setup(DeleteMate);

            void DeleteMate()
            {
                DormManager.Instance.RemoveDormMate(obj);
                Setup();
            }
        }

        public void Setup()
        {
            foreach (Transform child in container)
                Destroy(child.gameObject);
            foreach (DormMate mate in DormManager.Instance.DormMates.Where(mate =>
                         mate.SleepIn == DormMateSleepIn.Dungeon))
                SetupDormMate(mate);
        }

        void SetupDormMate(DormMate mate)
        {
            var b = Instantiate(dormMateButton, container);
            b.Setup(mate);
            b.SelectedMe += ShowSelectedMate;
            b.DoYouWantToKick += TryKick;
            b.WantToFuck += Fuck;
        }

        void ShowSelectedMate(DormMateButton btn, DormMate obj) => viewSelected.Setup(btn, obj);
    }
}
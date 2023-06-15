using System.Linq;
using DormAndHome.Dorm;
using Safe_To_Share.Scripts.Holders;
using SaveStuff;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI {
    public sealed class ViewDormDormDungeon : MonoBehaviour {
        [SerializeField] PlayerHolder playerHolder;
        [SerializeField] DormDungeonMateButton dormMateButton;
        [SerializeField] AreYouSure areYouSure;
        [SerializeField] Transform container;
        [SerializeField] ViewSelectedDormMate viewSelected;
        void OnEnable() => Setup();
        void Fuck(DormMate obj) => SceneLoader.Instance.LoadAfterBattleFromLocation(playerHolder, obj);

        void TryKick(DormMate obj) {
            Instantiate(areYouSure, transform).Setup(DeleteMate);

            void DeleteMate() {
                DormManager.Instance.RemoveDormMate(obj);
                Setup();
            }
        }

        public void Setup() {
            foreach (Transform child in container)
                Destroy(child.gameObject);
            foreach (var mate in DormManager.Instance.DormMates.Where(mate =>
                         mate.SleepIn == DormMateSleepIn.Dungeon))
                SetupDormMate(mate);
        }

        void SetupDormMate(DormMate mate) {
            var b = Instantiate(dormMateButton, container);
            b.Setup(mate);
            b.SelectedMe += ShowSelectedMate;
            b.DoYouWantToKick += TryKick;
            b.WantToFuck += Fuck;
        }

        void ShowSelectedMate(DormMateButton btn, DormMate obj) => viewSelected.Setup(btn, obj);
    }
}
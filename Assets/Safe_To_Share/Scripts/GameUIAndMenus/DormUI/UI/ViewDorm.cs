using Character.EssenceStuff;
using DormAndHome.Dorm;
using DormAndHome.Dorm.Buildings;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Static;
using SaveStuff;
using SceneStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public sealed class ViewDorm : MonoBehaviour
    {
        [SerializeField] PlayerHolder playerHolder;
        [SerializeField] DormMateButton dormMateButton;
        [SerializeField] AreYouSure areYouSure;
        [SerializeField] Transform container;
        [SerializeField] TMP_Dropdown essenceStoneGain;
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
            DormManager dormManager = DormManager.Instance;
            foreach (DormMate mate in dormManager.DormMates)
                SetupDormMate(mate);
            DormEssenceStone essenceStone = dormManager.Buildings.DormLodge.EssenceStone;
            if (essenceStone.Level > 0)
            {
                essenceStoneGain.gameObject.SetActive(true);
                essenceStoneGain.SetupTmpDropDown(essenceStone.GainType, ChangeGainType);
            }
            else
                essenceStoneGain.gameObject.SetActive(false);
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

        static void ChangeGainType(int arg0) => DormManager.Instance.Buildings.DormLodge.EssenceStone.GainType =
            UgreTools.IntToEnum(arg0, DrainEssenceType.None);
    }
}
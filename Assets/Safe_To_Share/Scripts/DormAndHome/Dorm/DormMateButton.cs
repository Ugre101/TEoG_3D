using System;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DormAndHome.Dorm {
    public class DormMateButton : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] protected TextMeshProUGUI race;
        [SerializeField] protected TextMeshProUGUI desc;
        [SerializeField] protected Button selectBtn, kickOut, fuckBtn;
        protected DormMate Mate;
        public event Action<DormMate> DoYouWantToKick;
        public event Action<DormMate> WantToFuck;
        public event Action<DormMateButton, DormMate> SelectedMe;

        public virtual void Setup(DormMate dormMate) {
            Mate = dormMate;
            titleText.text = dormMate.TitleConversion();
            nameText.text = dormMate.Identity.FullName;
            race.text = dormMate.RaceSystem.Race.Title;
            var body = dormMate.Body;
            var shortDesc = $"{body.Height.Value.ConvertCm()} and {body.Weight.ConvertKg()}";
            desc.text = shortDesc;
            kickOut.onClick.AddListener(Kick);
            fuckBtn.onClick.AddListener(Fuck);
            selectBtn.onClick.AddListener(InvokeSelect);
        }

        void InvokeSelect() => SelectedMe?.Invoke(this, Mate);
        public void RefreshNames() => nameText.text = Mate.Identity.FullName;
        void Fuck() => WantToFuck?.Invoke(Mate);

        void Kick() => DoYouWantToKick?.Invoke(Mate);
    }
}
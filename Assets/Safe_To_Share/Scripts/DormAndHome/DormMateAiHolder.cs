using System.Linq;
using Character.BodyStuff;
using Character.PlayerStuff;
using DormAndHome.Dorm;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace AvatarStuff.Holders
{
    public class DormMateAiHolder : AiHolder, IInteractable
    {
        [SerializeField] DormMate mate;

        public DormMate Mate => mate;

        void OnDestroy() => UnSub();

        public string HoverText(Player player) => "Fuck";
        public void DoInteraction(Player player) => PlayerHolder.Instance.TriggerSex(Mate);

        public void AddMate(DormMate dormMate)
        {
            mate = dormMate;
            Sub();
            UpdateAvatar(Mate);
            HeightsChange(Mate.Body.Height.Value);
        }

        void IfPregnant(int obj)
        {
            if (Mate.SexualOrgans.Vaginas.List.Any(v => v.Womb.HasFetus))
                ModifyAvatar();
        }

        protected override void Sub()
        {
            base.Sub();
            Mate.UpdateAvatar += ModifyAvatar;
            foreach (BodyStat bodyStat in Mate.Body.BodyStats.Values)
                bodyStat.StatDirtyEvent += ModifyAvatar;
            DateSystem.NewDay += IfPregnant;
            Mate.Sub();
        }

        protected override void UnSub()
        {
            base.UnSub();
            Mate.UpdateAvatar -= ModifyAvatar;
            foreach (BodyStat bodyStat in Mate.Body.BodyStats.Values)
                bodyStat.StatDirtyEvent -= ModifyAvatar;
            DateSystem.NewDay -= IfPregnant;
            Mate.Unsub();
        }

        protected override void NewAvatar(CharacterAvatar obj)
        {
            Mate.UpdateAvatar -= ModifyAvatar;
            Changer.CurrentAvatar.Setup(Mate);
            Mate.UpdateAvatar += ModifyAvatar;
        }

        void ModifyAvatar() => Changer.CurrentAvatar.Setup(Mate);
    }
}
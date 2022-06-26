using AvatarStuff.Holders;
using Character.PlayerStuff;
using Character.RelationShipStuff;

namespace DormAndHome.Dorm.UI
{
    public class ViewSelectedDungeonDormMate : ViewSelectedDormMate
    {
        protected override void ChangeSleepArea()
        {
            if (DormMateIsNull || !CanSendUp(SelectedMate))
                return;
            SelectedMate.SleepIn = DormMateSleepIn.Lodge;
            Destroy(SelectedButton.gameObject);
            Clear();
        }

        protected override void CanChangeArea() => changeSleepingArea.gameObject.SetActive(CanSendUp(SelectedMate));

        static bool CanSendUp(DormMate dormMate)
        {
            var relWithPlayer = dormMate.RelationsShips.GetRelationShipWith(PlayerHolder.PlayerID);
            var aff = relWithPlayer.Affection;
            var sub = relWithPlayer.Submission;

            if (aff > RelationShipExtensions.SecondThreesHold)
                return true;
            if (sub > RelationShipExtensions.SecondThreesHold)
                return true;
            return aff > -RelationShipExtensions.FirstThreesHold && sub > -RelationShipExtensions.FirstThreesHold;
        }
    }
}
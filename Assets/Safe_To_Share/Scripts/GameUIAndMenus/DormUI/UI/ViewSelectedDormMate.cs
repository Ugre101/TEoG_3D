using System.Text;
using AvatarStuff.Holders;
using Character;
using Character.BodyStuff;
using Character.GenderStuff;
using Character.Race;
using Character.RelationShipStuff;
using Safe_To_Share.Scripts.Holders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DormAndHome.Dorm.UI
{
    public class ViewSelectedDormMate : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI fullName, bodyDesc, raceDesc, titleDesc;
        [SerializeField] protected Button rename;
        [SerializeField] protected RenameDormMate renameDormMate;
        [SerializeField] protected Button changeSleepingArea;

        protected bool DormMateIsNull;
        protected DormMateButton SelectedButton;
        protected DormMate SelectedMate;

        protected virtual void Start()
        {
            rename.onClick.AddListener(RenameMate);
            changeSleepingArea.onClick.AddListener(ChangeSleepArea);
        }

        protected void OnEnable() => Clear();

        protected void OnDisable() => Clear();

        void RenameMate()
        {
            if (DormMateIsNull)
                return;
            renameDormMate.Setup(SelectedMate);
        }

        public void Clear()
        {
            SelectedMate = null;
            DormMateIsNull = true;
            titleDesc.text = string.Empty;
            fullName.text = string.Empty;
            raceDesc.text = string.Empty;
            bodyDesc.text = "Click on a follower to select";
            changeSleepingArea.gameObject.SetActive(false);
        }

        public virtual void Setup(DormMateButton dormMateButton, DormMate dormMate)
        {
            SelectedButton = dormMateButton;
            SelectedMate = dormMate;
            DormMateIsNull = false;
            PrintName();
            PrintTitleDesc();
            bodyDesc.text = SelectedMate.BodyDesc(false);
            PrintRaceInfo(dormMate);
            CanChangeArea();
        }

        protected virtual void CanChangeArea() =>
            changeSleepingArea.gameObject.SetActive(DormManager.Instance.Buildings.Dungeon.Level > 0);

        void PrintTitleDesc()
        {
            if (DormMateIsNull)
                return;
            StringBuilder sb = new();
            sb.AppendLine($"You treat {SelectedMate.Gender.HimHer()} as a {SelectedMate.TitleConversion()}.");
            RelationShip relaWithPlayer = SelectedMate.RelationsShips.GetRelationShipWith(PlayerHolder.PlayerID);
            sb.Append($"Affection {relaWithPlayer.Affection}");
            sb.Append("\t");
            sb.Append($"Submission {relaWithPlayer.Submission}");
            titleDesc.text = sb.ToString();
        }

        void PrintRaceInfo(BaseCharacter dormMate)
        {
            if (DormMateIsNull)
                return;
            StringBuilder sb = new();
            sb.Append(dormMate.Identity.FirstName);
            sb.Append(" is a ");
            sb.AppendLine(SelectedMate.RaceSystem.Race.Title);
            sb.AppendLine();
            sb.Append("(");
            foreach (RaceEssence raceEssence in SelectedMate.RaceSystem.AllRaceEssence)
                sb.Append($"{raceEssence.Race.Title} {{{raceEssence.Amount}}}, ");
            sb.Append(")");
            raceDesc.text = sb.ToString();
        }

        protected virtual void ChangeSleepArea()
        {
            if (DormMateIsNull)
                return;
            SelectedMate.SleepIn = DormMateSleepIn.Dungeon;
            Destroy(SelectedButton.gameObject);
            Clear();
        }

        public void PrintName()
        {
            if (DormMateIsNull)
                return;
            fullName.text = SelectedMate.Identity.FullName;
        }

        public void RefreshSelectedButtonNames()
        {
            if (DormMateIsNull)
                return;
            SelectedButton.RefreshNames();
        }
    }
}
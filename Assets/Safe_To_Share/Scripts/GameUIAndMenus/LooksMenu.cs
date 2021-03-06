using System.Linq;
using System.Text;
using Character;
using Character.BodyStuff;
using Character.Organs.Fluids.UI;
using Character.Race.UI;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus
{
    public class LooksMenu : GameMenu
    {
        [SerializeField] TextMeshProUGUI summary;
        [SerializeField] RaceInfo raceInfo;
        [SerializeField] BodyInfo bodyInfo;
        [SerializeField] FluidInfo cumInfo, milkInfo;

        void OnEnable()
        {
            raceInfo.PrintRaceInfo(Player);
            bodyInfo.PrintBodyInfo(Player);
            cumInfo.UpdateFluid(Player.SexualOrgans.Balls, Player.Body.Height.Value);
            milkInfo.UpdateFluid(Player.SexualOrgans.Boobs, Player.Body.Height.Value);
            PrintSummary();
            transform.SleepChildren(transform.GetChild(0).gameObject);
        }

        void PrintSummary()
        {
            StringBuilder sb = new();
            sb.Append($"Your name is {Player.Identity.FullName} you an 18 years old {Player.RaceSystem.Race.Title}. ");
            sb.Append(Player.HeightDesc());

            if (Player.SexualOrgans.Vaginas.List.Any(v => v.Womb.HasFetus))
                sb.Append("\n\n\n You are currently pregnant");
            summary.text = sb.ToString();
        }
    }
}
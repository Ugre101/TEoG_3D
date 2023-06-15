using System.Text;
using TMPro;
using UnityEngine;

namespace Character.Race.UI {
    public sealed class RaceInfo : MonoBehaviour {
        [SerializeField] TextMeshProUGUI raceText;
        [SerializeField] TextMeshProUGUI moreInfo;

        public void PrintRaceInfo(BaseCharacter character) {
            StringBuilder sb = new("Race\n");
            if (character.RaceSystem.Race != null)
                sb.AppendLine(character.RaceSystem.Race.Title);
            if (character.RaceSystem.SecRace != null && character.RaceSystem.SecRace != character.RaceSystem.Race)
                sb.AppendLine($"Sec race:\n{character.RaceSystem.SecRace.Title}");
            raceText.text = sb.ToString();
            PrintMoreInfo(character);
        }

        void PrintMoreInfo(BaseCharacter character) {
            StringBuilder sb = new();
            foreach (var raceEssence in character.RaceSystem.AllRaceEssence)
                sb.AppendLine($"{raceEssence.Race.Title} {{{raceEssence.Amount}}}");
            moreInfo.text = sb.ToString();
        }
    }
}
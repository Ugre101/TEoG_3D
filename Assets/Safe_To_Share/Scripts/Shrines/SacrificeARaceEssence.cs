using Character.PlayerStuff;
using Character.Race;
using SaveStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Shrines {
    public sealed class SacrificeARaceEssence : MonoBehaviour {
        [SerializeField] TextMeshProUGUI raceTitle, amountText;

        Player player;
        RaceEssence race;
        AreYouSure areYouSure;
        public void Setup(RaceEssence race, Player player, AreYouSure areYouSure) {
            this.player = player;
            this.race = race;
            this.areYouSure = areYouSure;
            raceTitle.text = race.Race.Title;
            amountText.text = race.Amount.ToString();
        }

        public void OnClickButton() {
            areYouSure.Setup(RemoveRaceAndBtn);
        }

        void RemoveRaceAndBtn() {
            player.RaceSystem.RemoveRace(race.Race);
            Destroy(gameObject);
        }
    }
}
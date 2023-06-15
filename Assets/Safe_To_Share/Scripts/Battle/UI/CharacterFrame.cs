using Battle;
using Character;
using Character.StatsStuff.HealthStuff.UI;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.UI {
    public sealed class CharacterFrame : MonoBehaviour {
        [SerializeField] HealthSlider healthSlider, willPowerSlider;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] HealthChangePopupNumbers healthChangePopupNumbers;

        public void OnDestroy() {
            healthSlider.UnBindValueChange();
            willPowerSlider.UnBindValueChange();
            healthChangePopupNumbers.UnSub();
        }

        public void Setup(BaseCharacter character) {
            healthSlider.Setup(character.Stats.Health);
            willPowerSlider.Setup(character.Stats.WillPower);
            healthChangePopupNumbers.Setup(character.Stats.Health, character.Stats.WillPower);
            title.text = character.Identity.FullName;
        }
    }
}
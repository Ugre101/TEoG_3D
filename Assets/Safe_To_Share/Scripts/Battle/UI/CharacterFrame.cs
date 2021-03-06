using Character;
using Character.StatsStuff.HealthStuff.UI;
using TMPro;
using UnityEngine;

namespace Battle.UI
{
    public class CharacterFrame : MonoBehaviour
    {
        [SerializeField] HealthSlider healthSlider, willPowerSlider;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] HealthChangePopupNumbers healthChangePopupNumbers;

        public void Setup(BaseCharacter character)
        {
            healthSlider.Setup(character.Stats.Health);
            willPowerSlider.Setup(character.Stats.WillPower);
            healthChangePopupNumbers.Setup(character.Stats.Health, character.Stats.WillPower);
            title.text = character.Identity.FullName;
        }

        public void OnDestroy()
        {
            healthSlider.UnBindValueChange();
            willPowerSlider.UnBindValueChange();
            healthChangePopupNumbers.UnSub();
        }
    }
}
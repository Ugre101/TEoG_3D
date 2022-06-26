using Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarStuff.UI
{
    public class BodyMorphSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI value;

        BodyMorphs.AvatarBodyMorphs.BodyMorph myMorph;
        CharacterAvatar currentAvatar;
        public void Setup(BodyMorphs.AvatarBodyMorphs.BodyMorph morph, CharacterAvatar avatar)
        {
            myMorph = morph;
            currentAvatar = avatar;
            title.text = morph.title;
            value.text = Mathf.RoundToInt(morph.value).ToString();
            slider.onValueChanged.RemoveAllListeners();
            slider.SetValueWithoutNotify(morph.value);
            slider.onValueChanged.AddListener(ChangeValue);
        }

        void ChangeValue(float arg0)
        {
            myMorph.value = arg0;
            value.text = Mathf.RoundToInt(arg0).ToString();
            currentAvatar.UpdateABodyTypeMorphs(myMorph);
        }
    }
}
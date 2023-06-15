using AvatarStuff;
using Character;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Holders.UI {
    public sealed class BodyMorphSlider : MonoBehaviour {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI value;
        CharacterAvatar currentAvatar;
        bool hasPool;

        BodyMorphs.AvatarBodyMorphs.BodyMorph myMorph;
        IObjectPool<BodyMorphSlider> pool;

        void OnDisable() {
            if (hasPool)
                pool.Release(this);
        }

        public void Setup(BodyMorphs.AvatarBodyMorphs.BodyMorph morph, CharacterAvatar avatar,
                          IObjectPool<BodyMorphSlider> spawnPool) {
            myMorph = morph;
            currentAvatar = avatar;
            pool = spawnPool;
            hasPool = true;
            title.text = morph.title;
            value.text = Mathf.RoundToInt(morph.value).ToString();
            slider.onValueChanged.RemoveAllListeners();
            slider.SetValueWithoutNotify(morph.value);
            slider.onValueChanged.AddListener(ChangeValue);
        }

        void ChangeValue(float arg0) {
            myMorph.value = arg0;
            value.text = Mathf.RoundToInt(arg0).ToString();
            currentAvatar.UpdateABodyTypeMorphs(myMorph);
        }

        public void Clear() {
            myMorph = null;
            currentAvatar = null;
            title.text = string.Empty;
            value.text = string.Empty;
        }
    }
}
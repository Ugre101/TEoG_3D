using System;
using Character;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
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
        IObjectPool<BodyMorphSlider> pool;
        bool hasPool;
        public void Setup(BodyMorphs.AvatarBodyMorphs.BodyMorph morph, CharacterAvatar avatar,IObjectPool<BodyMorphSlider> spawnPool)
        {
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

        void OnDisable()
        {
            if (hasPool)
                pool.Release(this);
        }

        void ChangeValue(float arg0)
        {
            myMorph.value = arg0;
            value.text = Mathf.RoundToInt(arg0).ToString();
            currentAvatar.UpdateABodyTypeMorphs(myMorph);
        }

        public void Clear()
        {
            myMorph = null;
            currentAvatar = null;
            title.text = string.Empty;
            value.text = string.Empty;
        }
    }
}
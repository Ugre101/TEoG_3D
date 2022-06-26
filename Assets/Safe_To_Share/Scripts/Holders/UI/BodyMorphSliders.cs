using System.Collections.Generic;
using Character;
using UnityEngine;

namespace AvatarStuff.UI
{
    public class BodyMorphSliders : MonoBehaviour
    {
        [SerializeField] BodyMorphSlider prefab;
        [SerializeField] Transform content;

        Queue<BodyMorphSlider> sliderPool;

        public void Setup(BodyMorphs.AvatarBodyMorphs morphs, CharacterAvatar avatar)
        {
            SetupSliderPool();
            foreach (var subStruct in morphs.bodyAvatarMorphs)
                GetSlider().Setup(subStruct, avatar);
        }


        public void SetupOfType(BodyMorphs.AvatarBodyMorphs morphs, CharacterAvatar avatar,
            CharacterAvatar.BodyShapes.BodyShapeTypes type)
        {
            SetupSliderPool();
            foreach (var bodyShape in avatar.AvatarBodyShapes.GetBodyShapesOfType(type))
            {
                var found = morphs.bodyAvatarMorphs.Find(b => b.title == bodyShape.Title);
                if (found != null)
                    GetSlider().Setup(found, avatar);
            }
        }

        void SetupSliderPool()
        {
            sliderPool = new Queue<BodyMorphSlider>(content.GetComponentsInChildren<BodyMorphSlider>(true));
            content.SleepChildren();
        }

        BodyMorphSlider GetSlider()
        {
            var gotten = sliderPool.Count > 0 ? sliderPool.Dequeue() : Instantiate(prefab, content);
            gotten.gameObject.SetActive(true);
            return gotten;
        }
    }
}
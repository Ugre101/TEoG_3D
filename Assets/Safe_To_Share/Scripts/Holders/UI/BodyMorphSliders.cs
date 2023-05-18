using AvatarStuff;
using Character;
using UnityEngine;
using UnityEngine.Pool;

namespace Safe_To_Share.Scripts.Holders.UI
{
    public sealed class BodyMorphSliders : MonoBehaviour
    {
        [SerializeField] BodyMorphSlider prefab;
        [SerializeField] Transform content;

        bool firstSetup = true;

        ObjectPool<BodyMorphSlider> sliderPool;

        public void Setup(BodyMorphs.AvatarBodyMorphs morphs, CharacterAvatar avatar)
        {
            if (firstSetup)
                SetupSliderPool();
            foreach (var subStruct in morphs.bodyAvatarMorphs)
                sliderPool.Get().Setup(subStruct, avatar, sliderPool);
        }


        public void SetupOfType(BodyMorphs.AvatarBodyMorphs morphs, CharacterAvatar avatar,
            CharacterAvatar.BodyShapes.BodyShapeTypes type)
        {
            SetupSliderPool();
            foreach (var bodyShape in avatar.AvatarBodyShapes.GetBodyShapesOfType(type))
            {
                var found = morphs.bodyAvatarMorphs.Find(b => b.title == bodyShape.Title);
                if (found != null)
                    sliderPool.Get().Setup(found, avatar, sliderPool);
            }
        }

        void SetupSliderPool()
        {
            firstSetup = false;
            sliderPool = new ObjectPool<BodyMorphSlider>(CreateFunc, ActionOnGet, ActionOnRelease);
            foreach (var slider in GetComponentsInChildren<BodyMorphSlider>())
                sliderPool.Release(slider);
        }

        static void ActionOnRelease(BodyMorphSlider obj)
        {
            obj.Clear();
            obj.gameObject.SetActive(false);
        }

        static void ActionOnGet(BodyMorphSlider obj) => obj.gameObject.SetActive(true);

        BodyMorphSlider CreateFunc() => Instantiate(prefab, content);
    }
}
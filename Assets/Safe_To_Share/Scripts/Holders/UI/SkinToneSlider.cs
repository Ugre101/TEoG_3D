using AvatarStuff.Holders;
using Character.PlayerStuff;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarStuff.UI
{
    public class SkinToneSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;

        PlayerHolder playerHolder;
        void Start()
        {
            slider.onValueChanged.AddListener(Change);
        }

        void Change(float arg0)
        {
            playerHolder.CurrentAvatar.SetSkinTone(arg0);
            playerHolder.Player.Body.SkinTone = arg0;
        }

        public void Setup(PlayerHolder holder)
        {
            playerHolder = holder;
            slider.SetValueWithoutNotify(holder.Player.Body.SkinTone);
        }
    }
}
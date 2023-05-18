using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Holders.UI
{
    public sealed class SkinToneSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;

        PlayerHolder playerHolder;

        void Start() => slider.onValueChanged.AddListener(Change);

        void Change(float arg0)
        {
            playerHolder.Changer.CurrentAvatar.SetSkinTone(arg0,false);
            playerHolder.Player.Body.SkinTone = arg0;
        }

        public void Setup(PlayerHolder holder)
        {
            playerHolder = holder;
            slider.SetValueWithoutNotify(holder.Player.Body.SkinTone);
        }
    }
}
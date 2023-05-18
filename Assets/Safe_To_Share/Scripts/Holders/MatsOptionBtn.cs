using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Holders
{
    public sealed class MatsOptionBtn : MonoBehaviour
    {
        [SerializeField] Button btn;
        [SerializeField] Image btnBackground;
        [SerializeField] TextMeshProUGUI btnTitle;
        bool chosen = true;
        Material material;

        void Start()
        {
            btn.onClick.AddListener(Click);
            ChangeAvatarDetails.ToggleAll += ToggledAll;
        }

        void OnDestroy() => ChangeAvatarDetails.ToggleAll -= ToggledAll;

        public event Action<Material> AddMe;
        public event Action<Material> RemoveMe;

        public void Setup(Material mat)
        {
            material = mat;
            btnTitle.text = mat.name;
            UpdateChosenColor();
        }

        void ToggledAll(bool obj)
        {
            chosen = obj;
            UpdateChosenColor();
        }

        void Click()
        {
            chosen = !chosen;
            if (chosen)
                AddMe?.Invoke(material);
            else
                RemoveMe?.Invoke(material);
            UpdateChosenColor();
        }

        void UpdateChosenColor() => btnBackground.color = chosen ? Color.green : Color.gray;
    }
}
using System;
using System.Collections.Generic;
using AvatarStuff.Holders;
using AvatarStuff.UI;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarStuff
{
    public class ChangeAvatarDetails : MonoBehaviour
    {
        [SerializeField] GetColor colorPicker;
        [SerializeField] MatsOptionBtn btnPrefab;
        [SerializeField] Transform matsBtnsContainer;
        [SerializeField] Button chooseAllBtn;
        [SerializeField] Button toggleBaldBtn;

        [SerializeField] SkinToneSlider skinToneSlider;

        Material[] canChangeColorOff;
        List<Material> changeColorOff = new();
        CharacterAvatar currentAvatar;
        PlayerHolder holder;

        void Start()
        {
            chooseAllBtn.onClick.AddListener(ChooseAll);
            toggleBaldBtn.onClick.AddListener(ToggleBald);
        }

        void OnDisable()
        {
            colorPicker.NewColor -= UpdateColor;
            currentAvatar.Save();
        }

        public void Enter(PlayerHolder playerHolder)
        {
            holder = playerHolder;
            gameObject.SetActive(true);
            GetAvatarMats();
            SetupChooseMats();
            colorPicker.NewColor += UpdateColor;
            skinToneSlider.Setup(holder);
        }

        public static event Action<bool> ToggleAll;

        void ToggleBald()
        {
            holder.Player.Hair.Bald = !holder.Player.Hair.Bald;
            holder.ModifyCurrentAvatar();
        }

        void ChooseAll()
        {
            bool allToggled = changeColorOff.Count == canChangeColorOff.Length;
            changeColorOff = allToggled ? new List<Material>() : new List<Material>(canChangeColorOff);
            ToggleAll?.Invoke(!allToggled);
        }

        void SetupChooseMats()
        {
            matsBtnsContainer.KillChildren();
            foreach (var mat in canChangeColorOff)
                SetupMatBtn(mat);
        }

        void SetupMatBtn(Material mat)
        {
            var btn = Instantiate(btnPrefab, matsBtnsContainer);
            btn.Setup(mat);
            btn.AddMe += AddMat;
            btn.RemoveMe += RemoveMat;
        }

        void RemoveMat(Material obj) => changeColorOff.Remove(obj);

        void AddMat(Material obj)
        {
            if (changeColorOff.Contains(obj))
                return;
            changeColorOff.Add(obj);
        }

        void UpdateColor(Color obj)
        {
            foreach (Material material in changeColorOff)
                material.color = obj;
        }

        [ContextMenu("Get mats")]
        public void GetAvatarMats()
        {
            currentAvatar = holder.Changer.CurrentAvatar;
            canChangeColorOff = currentAvatar.HairMats;
            changeColorOff = new List<Material>(canChangeColorOff);
        }
    }
}
using System;
using Character.EssenceStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Organs.UI {
    public sealed class GrowOrganButton : MonoBehaviour {
        public static bool Change;
        [SerializeField] Button btn;
        [SerializeField] Button altShrinkBtn;
        [SerializeField] TextMeshProUGUI text;
        Essence essence;
        EssenceType essenceType;
        BaseOrgan organ;
        SexualOrganType organType;

        ManualOrganGrowth.SharedInfo shared;

        public void Setup(BaseOrgan org, ManualOrganGrowth.SharedInfo shared, bool canShrink = false) {
            organ = org;
            this.shared = shared;
            essence = shared.essence;
            organType = shared.organType;
            essenceType = shared.essenceType;
            UpdateText();
            btn.onClick.AddListener(Grow);
            altShrinkBtn.onClick.AddListener(Shrink);
            altShrinkBtn.gameObject.SetActive(canShrink);
        }

        public event Action<SexualOrganType, BaseOrgan> RecycleMe;

        void Shrink() {
            essence.GainEssence(Mathf.RoundToInt(organ.Shrink() * 0.7f));
            UpdateText();
            Change = true;
            if (organ.BaseValue <= 0) {
                RecycleMe?.Invoke(organType, organ);
                Destroy(gameObject);
            }
        }

        void UpdateText() =>
            text.text =
                $"{organType} {organ.ScaledWithHeight(shared.height).ConvertCm()} {organ.GrowCost}{essenceType}";

        void Grow() {
            if (!organ.Grow(essence))
                return;
            UpdateText();
            Change = true;
        }
    }
}
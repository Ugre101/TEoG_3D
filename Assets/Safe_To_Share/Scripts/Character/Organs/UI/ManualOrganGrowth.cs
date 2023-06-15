using System;
using System.Linq;
using Character.EssenceStuff;
using Character.Organs.OrgansContainers;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Organs.UI {
    public sealed class ManualOrganGrowth : MonoBehaviour {
        public static bool Change;
        [SerializeField] Button growNew;
        [SerializeField] TextMeshProUGUI growNewTitle;
        [SerializeField] Transform content;
        [SerializeField] GrowOrganButton growOrganButton;
        [SerializeField] SexualOrganType organType;
        [SerializeField] EssenceType essenceType;
        bool canRecycle;
        BaseCharacter character;
        SharedInfo shared;

        public void Setup(BaseCharacter baseCharacter) {
            foreach (Transform child in content)
                Destroy(child.gameObject);
            growNew.onClick.RemoveAllListeners();
            character = baseCharacter;
            if (!character.SexualOrgans.Containers.TryGetValue(organType, out var organsContainer) ||
                !character.Essence.GetEssence.TryGetValue(essenceType, out var essence))
                return;
            shared = new SharedInfo(essence, essenceType, organType, baseCharacter.Body.Height.Value);
            UpdateNewCost(organsContainer);
            SetupGrowNew(organsContainer);
            canRecycle = character.Essence.EssencePerks.OfType<OrganReCyclePerk>().Any();
            foreach (var organ in organsContainer.BaseList) {
                var btn = Instantiate(growOrganButton, content);
                btn.Setup(organ, shared, canRecycle);
                btn.RecycleMe += RemoveOrgan;
            }
        }

        void SetupGrowNew(BaseOrgansContainer baseOrgansContainer) {
            if (baseOrgansContainer.HaveAny() && !OptionalContent.MultiOrgan.Enabled) {
                growNew.gameObject.SetActive(false);
                return;
            }

            growNew.gameObject.SetActive(true);
            growNew.onClick.AddListener(GrowNew);
        }

        void RemoveOrgan(SexualOrganType arg1, BaseOrgan arg2) {
            if (!character.SexualOrgans.Containers.TryGetValue(arg1, out var container))
                return;
            switch (arg1) {
                case SexualOrganType.Dick:
                    break;
                case SexualOrganType.Balls:
                    break;
                case SexualOrganType.Boobs:
                    break;
                case SexualOrganType.Vagina:
                    break;
                case SexualOrganType.Anal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(arg1), arg1, null);
            }

            if (!container.RemoveOrgan(arg2))
                return;
            if (!character.Essence.GetEssence.TryGetValue(essenceType, out var ess))
                return;
            ess.GainEssence(Mathf.RoundToInt(container.GrowNewCost * 0.7f));
        }

        void UpdateNewCost(BaseOrgansContainer baseOrgansContainer) =>
            growNewTitle.text = $"Grow new {organType} {baseOrgansContainer.GrowNewCost}{essenceType}";

        void GrowNew() {
            if (!character.SexualOrgans.Containers.TryGetValue(organType, out var organsContainer) ||
                !character.Essence.GetEssence.TryGetValue(essenceType, out var essence) ||
                !organsContainer.TryGrowNew(essence))
                return;
            var newOrgan = organsContainer.BaseList.LastOrDefault();
            var btn = Instantiate(growOrganButton, content);
            btn.Setup(newOrgan, shared, canRecycle);
            btn.RecycleMe += RemoveOrgan;
            if (OptionalContent.MultiOrgan.Enabled)
                UpdateNewCost(organsContainer);
            else
                growNew.gameObject.SetActive(false);
            Change = true;
        }

        public struct SharedInfo {
            public Essence essence;
            public EssenceType essenceType;
            public SexualOrganType organType;
            public float height;

            public SharedInfo(Essence essence, EssenceType essenceType, SexualOrganType organType, float height) {
                this.essence = essence;
                this.essenceType = essenceType;
                this.organType = organType;
                this.height = height;
            }
        }
    }
}
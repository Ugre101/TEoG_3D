using Character.Organs.OrgansContainers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Organs.Fluids.UI
{
    public class FluidInfo : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI amount;

        public void UpdateFluid(BaseOrgansContainer baseOrgansContainer, float bodyHeight)
        {
            gameObject.SetActive(baseOrgansContainer.Fluid.Recovery.Value > 0 || baseOrgansContainer.Fluid.CurrentValue > 0);
            slider.maxValue = baseOrgansContainer.Fluid.Value;
            slider.value = baseOrgansContainer.Fluid.CurrentValue;
            string fluidInText = SexualOrgansExtensions.FluidAmountInText(baseOrgansContainer.FluidCurrent, bodyHeight);
            amount.text = fluidInText;
            title.text = baseOrgansContainer.FluidType;
        }
    }
}
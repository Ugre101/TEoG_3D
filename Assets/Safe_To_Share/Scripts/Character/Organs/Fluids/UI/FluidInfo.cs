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

        public void UpdateFluid(OrgansContainer organsContainer, float bodyHeight)
        {
            gameObject.SetActive(organsContainer.Fluid.Recovery.Value > 0 || organsContainer.Fluid.CurrentValue > 0);
            slider.maxValue = organsContainer.Fluid.Value;
            slider.value = organsContainer.Fluid.CurrentValue;
            string fluidInText = SexualOrgansExtensions.FluidAmountInText(organsContainer.FluidCurrent, bodyHeight);
            amount.text = fluidInText;
            title.text = organsContainer.FluidType;
        }
    }
}
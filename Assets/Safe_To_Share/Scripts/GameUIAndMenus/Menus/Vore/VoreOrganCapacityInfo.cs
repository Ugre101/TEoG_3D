using Character.VoreStuff;
using Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.Menus.Vore
{
    public class VoreOrganCapacityInfo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, desc;

        public void Setup(string titleText, VoreOrgan voreOrgan, float capacity)
        {
            title.text = titleText;
            float pretTot = voreOrgan.PreysIds.Count > 0
                ? VoredCharacters.CurrentPreyTotalWeight(voreOrgan.PreysIds)
                : 0f;
            desc.text = $"{pretTot.ConvertKg(false)} / {capacity.ConvertKg()}";
        }
    }
}
using System.Text;
using Character;
using Character.Organs.Fluids;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Looks
{
    public class SexualFluidsOnBodyInfo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI infoText;
        public void Setup(BaseCharacter character)
        {
            StringBuilder sb = new();
            sb.Append(character.FluidOnBodyDesc());
            foreach (var (key, value) in character.SexualOrgans.Containers)
            {
                foreach (var baseOrgan in value.BaseList)
                { 
                    sb.Append(baseOrgan.FluidInWomb(key));
                }
            }

            infoText.text = sb.ToString();
        }
    }
}
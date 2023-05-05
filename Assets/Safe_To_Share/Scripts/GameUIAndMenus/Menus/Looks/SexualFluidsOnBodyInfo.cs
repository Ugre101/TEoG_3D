using System.Linq;
using System.Text;
using Character;
using Character.Organs.Fluids;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.Menus.Looks
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
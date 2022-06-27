using System;
using System.Text;
using Character.GenderStuff;
using Character.VoreStuff;
using Character.VoreStuff.VoreDigestionModes;
using Character.VoreStuff.VoreDigestionModes.Vagina;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.Menus.Vore
{
    public class PreyShowCase : MonoBehaviour
    {
        public event Action<int,VoreType> RegurgitateMe;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI digestionModeInfo;
        [SerializeField] Button regurgitateBtn;

        int myPrey;
        VoreType type;
        void Start()
        {
            regurgitateBtn.onClick.AddListener(Regurgitate);
        }

        void Regurgitate()
        {
            RegurgitateMe?.Invoke(myPrey, type);
            Destroy(gameObject);
        }

        public void Setup(Prey prey,VoreType voreType, string mode)
        {
            myPrey = prey.Identity.ID;
            type = voreType;
            title.text = prey.Identity.FullName;
            digestionModeInfo.text = DigestionModeProgress(prey, voreType, mode);
        }

        static string DigestionModeProgress(Prey prey, VoreType voreType, string mode)
        {
            if (!string.IsNullOrEmpty(prey.SpecialDigestion))
                mode = prey.SpecialDigestion;
            StringBuilder sb = new();
            switch (mode)
            {
                case VoreOrganDigestionMode.Endo:
                    sb.Append($"Is resting inside your {Inside()}");
                    string Inside() => voreType switch
                    {
                        VoreType.Oral => "stomach",
                        VoreType.Balls => "balls",
                        VoreType.UnBirth => "womb",
                        VoreType.Anal => "",
                        VoreType.Breast => "boobs",
                        VoreType.Cock => "cock",
                        _ => throw new ArgumentOutOfRangeException(nameof(voreType), voreType, null)
                    };
                    return sb.ToString();
                case VoreOrganDigestionMode.Digestion:
                    sb.Append(
                        $"Is {prey.DigestionProgress}% digested currently weighting {prey.Body.Weight.ConvertKg()}");
                    return sb.ToString();
                case VoreOrganDigestionMode.Absorption:
                    sb.Append($"{prey.Gender.HeShe()} is {prey.AltProgress:#0.##}% merged into your {Into()}");
                    string Into() => voreType switch
                    {
                        VoreType.Oral => "body",
                        VoreType.Balls => "balls",
                        VoreType.UnBirth => "vagina",
                        VoreType.Anal => "body",
                        VoreType.Breast => "boobs",
                        VoreType.Cock => "cock",
                        _ => throw new ArgumentOutOfRangeException(nameof(voreType), voreType, null)
                    };
                    return sb.ToString();
                default:
                    switch (voreType)
                    {
                        case VoreType.Oral:
                            break;
                        case VoreType.Balls:
                            break;
                        case VoreType.UnBirth:
                            if (mode == VaginaDigestionModes.Rebirth)
                                sb.Append($"Has regressed {prey.AltProgress:#0.##}% towards becoming your child");
                            return sb.ToString();
                        case VoreType.Anal:
                            break;
                        case VoreType.Breast:
                            break;
                        case VoreType.Cock:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(voreType), voreType, null);
                    }
                    break;
            }
            return sb.ToString();
        }
    }
}

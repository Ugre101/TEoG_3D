using System.Text;
using Character.BodyStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Character
{
    public sealed class BodyInfo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        public void PrintBodyInfo(BaseCharacter character)
        {
            StringBuilder sb = new();
            Body body = character.Body;
            sb.AppendLine($"Height {body.Height.Value.ConvertCm()}");
            sb.AppendLine();
            sb.AppendLine($"Muscle {body.MuscleWeight.ConvertKg()}");
            sb.AppendLine();
            sb.AppendLine($"Fat {body.FatWeight.ConvertKg()}");
            sb.AppendLine();
            sb.AppendLine($"Weight {body.Weight.ConvertKg()}");
            text.text = sb.ToString();
        }
    }
}
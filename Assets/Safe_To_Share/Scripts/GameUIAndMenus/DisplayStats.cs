using System.Text;
using Character;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class DisplayStats : MonoBehaviour {
        [SerializeField] TextMeshProUGUI statText;

        void PrintStats(BaseCharacter character) {
            var sb = new StringBuilder();
            foreach ((var key, var value) in character.Stats.GetCharStats)
                sb.AppendLine($"{key}: {value.Value}");
            statText.text = sb.ToString();
        }
    }
}